using Messenger.HistoricalMessagesService.Models;
using Messenger.HistoricalMessagesService.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;

namespace Messenger.HistoricalMessagesService.Controllers
{
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

        [HttpPost("echo")]
        [Produces("application/json")]
        public Task<string> GetLastMessages(string echo)
        {
            return Task.FromResult(echo);
        }

        [HttpPost("get_last_messages")]
        [Produces("application/json")]
        public async Task<MessagesResponse> GetLastMessages(GetLastMessagesRequest request)
        {
            m_logger.Trace($"Retrieving messages of chat {request.ChatId}");

            MessagesResponse messagesResponse = await m_esInteractor.GetLastMessagesOfChatAsync(request.ChatId);
            return messagesResponse;
        }
    }
}
