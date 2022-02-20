using Messenger.SubscribingService.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.SubscribingService
{
    public class WebsocketConnectionHandler
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private readonly MessageHub m_messageHub;
        private WebSocket m_webSocketConnection;
        private int m_userId;
        private Guid m_guid = Guid.NewGuid();

        public Action<int, Guid, WebsocketConnectionHandler> OnClose;
        private bool m_closedEventFired = false;

        public WebsocketConnectionHandler(MessageHub messageHub)
        {
            m_messageHub = messageHub;
        }

        public async Task Handle(WebSocket webSocketConnection, int userId)
        {
            m_logger.Info($"Handling connection from {userId}. Connection id {m_guid}");
            m_userId = userId;
            m_webSocketConnection = webSocketConnection;
            await m_messageHub.Register(this, userId, m_guid);

            await SendServerMessage(new MessageFromServer
            {
                SystemMessage = "Hello from server!"
            });

            try
            {
                await Echo(webSocketConnection);
            }
            catch (Exception ex)
            {
                m_logger.Trace(ex);
            }
        }

        private async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            if (webSocket.State == WebSocketState.Open ||
                webSocket.State == WebSocketState.CloseReceived ||
                webSocket.State == WebSocketState.CloseSent)
            {
                m_logger.Info($"Closing websocket. Guid {m_guid}. User {m_userId}");
                await webSocket.CloseAsync(
                    receiveResult.CloseStatus.Value,
                    receiveResult.CloseStatusDescription,
                    CancellationToken.None);

                IsConnectionClosed();
            }
        }

        internal Guid GetGuid()
        {
            return m_guid;
        }

        public async Task SendServerMessage(MessageFromServer messageFromServer)
        {
            string message = JsonConvert.SerializeObject(messageFromServer, Formatting.None);
            await SendText(message);
        }

        internal async Task Close()
        {
            if (IsConnectionClosed())
            {
                return;
            }

            try
            {
                await m_webSocketConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, null, default);
            }
            catch (Exception ex)
            {
                m_logger.Error("Failed to close connection: " + ex.Message);
            }
        }

        private async Task SendText(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await SendBuffer(buffer);
        }

        private async Task SendBuffer(byte[] buffer)
        {
            if (IsConnectionClosed())
            {
                return;
            }

            try
            {
                await m_webSocketConnection.SendAsync(buffer, WebSocketMessageType.Text, true, default);
            }
            catch (Exception ex)
            {
                m_logger.Warn($"Failed to send message to websocket: {ex.Message}");
            }
        }

        private bool IsConnectionClosed()
        {
            if (m_closedEventFired)
            {
                return true;
            }

            if (m_webSocketConnection.State == WebSocketState.Closed ||
                m_webSocketConnection.State == WebSocketState.CloseReceived ||
                m_webSocketConnection.State == WebSocketState.CloseSent)
            {
                m_logger.Info($"Client {m_userId} closed connection {m_guid} without close sequence");
                m_closedEventFired = true;
                OnClose?.Invoke(m_userId, m_guid, this);
                return true;
            }

            return false;
        }
    }
}
