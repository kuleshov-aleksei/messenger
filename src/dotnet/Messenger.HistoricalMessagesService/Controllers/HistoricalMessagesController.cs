using Messenger.Common.Elastic;
using Messenger.Common.JWT;
using Messenger.Common.Models;
using Messenger.Common.MySql;
using Messenger.Common.Settings;
using Messenger.HistoricalMessagesService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.HistoricalMessagesService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/messenger/historical")]
    public class HistoricalMessagesController : ControllerBase
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private readonly EsInteractor m_esInteractor;
        private readonly IRedisDatabase m_redis;

        public HistoricalMessagesController(EsInteractor esInteractor, IRedisDatabase redisDatabase)
        {
            m_esInteractor = esInteractor;
            m_redis = redisDatabase;
        }

        // Add [AllowAnonymous] to skip authorization
        [HttpPost("get_last_messages")]
        [Produces("application/json")]
        public async Task<ChatMessages> GetLastMessages(GetLastMessagesRequest request)
        {
            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Trace($"User {userId} requested messages of chat {request.ChatId}");

            List<int> userChats = await GlobalSettings.Instance.Database.GetUserChatListAsync(m_redis, userId);
            if (!userChats.Contains(request.ChatId))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }

            ChatMessages messagesResponse = await m_esInteractor.GetLastMessagesOfChatAsync(request.ChatId);
            return messagesResponse;
        }

        [HttpPost("get_messages_from")]
        [Produces("application/json")]
        public async Task<ChatMessages> GetMessagesFrom(GetMessagesFromRequest request)
        {
            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Trace($"User {userId} requested messages of chat {request.ChatId} from {request.TimeFrom}");

            List<int> userChats = await GlobalSettings.Instance.Database.GetUserChatListAsync(m_redis, userId);
            if (!userChats.Contains(request.ChatId))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }

            ChatMessages messagesResponse = await m_esInteractor.GetMessagesFromAsync(request.TimeFrom, request.ChatId);
            return messagesResponse;
        }
    }
}
