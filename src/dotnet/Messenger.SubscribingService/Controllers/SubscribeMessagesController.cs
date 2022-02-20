using Messenger.Common.JWT;
using Messenger.SubscribingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.SubscribingService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/messenger/subscribe")]
    public class SubscribeMessagesController : ControllerBase
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private readonly WebsocketConnectionHandler m_websocketConnectionHandler;

        public SubscribeMessagesController(WebsocketConnectionHandler websocketConnectionHandler)
        {
            m_websocketConnectionHandler = websocketConnectionHandler;
        }

        [HttpGet("ws")]
        public async Task SubscribeWs()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                int userId = JwtHelper.GetUserId(User.Claims);
                m_logger.Info($"Client {userId} connected over websockets");
                using (WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    await m_websocketConnectionHandler.Handle(webSocket, userId);
                }
            }
            else
            {
                m_logger.Info($"Client tried to connect to ws endpoint, but connection isn't websocket");
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
