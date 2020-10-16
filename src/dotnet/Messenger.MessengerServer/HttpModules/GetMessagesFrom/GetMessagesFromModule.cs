using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.HttpModules.GetMessagesFrom
{
    public class GetMessagesFromModule : ModuleBase<GetMessagesFromRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        private EsInteractor m_esInteractor;

        public GetMessagesFromModule(EsInteractor esInteractor, JwtHelper jwtHelper)
            : base(Routes.GET_MESSAGES_FROM, jwtHelper)
        {
            m_esInteractor = esInteractor;
        }

        // TODO: Add user validation
        protected override async Task OnRequest(IHttpContext context, GetMessagesFromRequest request, int userId)
        {
            m_logger.Trace($"Retrieving messages of chat {request.ChatId} from {request.UnixTime}");

            MessagesResponse response = m_esInteractor.GetMessagesFrom(request.UnixTime, request.ChatId);

            await SendResponse(context, HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }
    }
}
