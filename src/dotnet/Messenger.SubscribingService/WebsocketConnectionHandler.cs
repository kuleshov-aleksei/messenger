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

        public WebsocketConnectionHandler(MessageHub messageHub)
        {
            m_messageHub = messageHub;
        }

        public async Task Handle(WebSocket webSocketConnection, int userId)
        {
            m_logger.Info($"Handling connection from {userId}");
            m_webSocketConnection = webSocketConnection;
            await m_messageHub.Register(this, userId);

            await webSocketConnection.SendAsync(Encoding.UTF8.GetBytes("Hello from server!"), WebSocketMessageType.Text, true, default);
            await Echo(webSocketConnection);
        }

        private static async Task Echo(WebSocket webSocket)
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
                await webSocket.CloseAsync(
                    receiveResult.CloseStatus.Value,
                    receiveResult.CloseStatusDescription,
                    CancellationToken.None);
            }
        }

        public async Task SendServerMessage(MessageFromServer messageFromServer)
        {
            string message = JsonConvert.SerializeObject(messageFromServer, Formatting.None);
            await SendText(message);
        }

        internal async Task Close()
        {
            await m_webSocketConnection.CloseAsync(WebSocketCloseStatus.NormalClosure, null, default);
        }

        private async Task SendText(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await SendBuffer(buffer);
        }

        private async Task SendBuffer(byte[] buffer)
        {
            await m_webSocketConnection.SendAsync(buffer, WebSocketMessageType.Text, true, default);
        }
    }
}
