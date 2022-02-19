using Messenger.SubscribingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.SubscribingService.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("/messenger/subscribe")]
    public class SubscribeMessagesController : ControllerBase
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public SubscribeMessagesController()
        {
        }

        [HttpGet("ws")]
        public async Task SubscribeWs()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                m_logger.Info($"Client connected over websockets");
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Echo(webSocket);
            }
            else
            {
                m_logger.Info($"Client tried to connect to ws endpoint, but connection isn't websocket");
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
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

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
