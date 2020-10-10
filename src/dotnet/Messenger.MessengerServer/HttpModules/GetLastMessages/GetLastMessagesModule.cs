using EmbedIO;
using Messenger.Common.Http;
using Newtonsoft.Json;
using NLog;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.HttpModules.GetLastMessages
{
    public class GetLastMessagesModule : ModuleBase<GetLastMessagesRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private EsInteractor m_esInteractor;

        public override bool IsFinalHandler => true;

        public GetLastMessagesModule(EsInteractor esInteractor)
            :base (Routes.GET_LAST_MESSAGES)
        {
            m_esInteractor = esInteractor;
        }

        protected override async Task OnRequest(IHttpContext context, GetLastMessagesRequest request)
        {
            m_logger.Trace($"Retrieving messages of chat {request.ChatId}");

            MessagesResponse messagesResponse = m_esInteractor.GetLastMessagesOfChat(request.ChatId);

            await SendResponse(context, HttpStatusCode.OK, JsonConvert.SerializeObject(messagesResponse));
        }
    }
}
