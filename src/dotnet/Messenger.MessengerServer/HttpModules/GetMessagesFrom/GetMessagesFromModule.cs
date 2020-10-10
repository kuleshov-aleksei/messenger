using EmbedIO;
using Messenger.Common.Http;
using Newtonsoft.Json;
using NLog;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.HttpModules.GetMessagesFrom
{
    public class GetMessagesFromModule : ModuleBase<GetMessagesFromRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        private EsInteractor m_esInteractor;

        public GetMessagesFromModule(EsInteractor esInteractor)
            : base(Routes.GET_MESSAGES_FROM)
        {
            m_esInteractor = esInteractor;
        }

        protected override async Task OnRequest(IHttpContext context, GetMessagesFromRequest request)
        {
            m_logger.Trace($"Retrieving messages of chat {request.ChatId} from {request.UnixTime}");

            MessagesResponse response = m_esInteractor.GetMessagesFrom(request.UnixTime, request.ChatId);

            await SendResponse(context, HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }
    }
}
