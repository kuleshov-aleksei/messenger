using Messenger.Common.JWT;
using Messenger.Common.Settings;
using Messenger.InstantMessagingService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Common;
using NLog;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.InstantMessagingService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/messenger/instant")]
    public class InstantMessagesController : ControllerBase
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private readonly IRedisDatabase m_redis;

        public InstantMessagesController(IRedisDatabase redisDatabase)
        {
            m_redis = redisDatabase;
        }

        [HttpPost("send")]
        public async Task SendMessage(SendMessageRequest request)
        {
            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Trace($"User {userId} is sending message to chat {request.ChatId}");

            List<int> userChats = await GlobalSettings.Instance.Database.GetUserChatListAsync(m_redis, userId);
            if (!userChats.Contains(request.ChatId))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            if (string.IsNullOrEmpty(request.Message))
            {
                m_logger.Debug("Message is empty. Rejecting");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            m_logger.Trace($"TEMP: {request.Message}");
        }
    }
}
