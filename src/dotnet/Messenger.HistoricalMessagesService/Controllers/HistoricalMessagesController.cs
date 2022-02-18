using Messenger.Common.JWT;
using Messenger.HistoricalMessagesService.Models;
using Messenger.HistoricalMessagesService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
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

        public HistoricalMessagesController(EsInteractor esInteractor)
        {
            m_esInteractor = esInteractor;
        }

        // Add [AllowAnonymous] to skip authorization

        [HttpPost("get_last_messages")]
        [Produces("application/json")]
        public async Task<MessagesResponse> GetLastMessages(GetLastMessagesRequest request)
        {
            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Trace($"User {userId} requested messages of chat {request.ChatId}");

            MessagesResponse messagesResponse = await m_esInteractor.GetLastMessagesOfChatAsync(request.ChatId);
            return messagesResponse;
        }
    }
}
