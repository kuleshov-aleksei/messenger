using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.HttpModules.GetLastMessages
{
    public class GetLastMessagesModule : ModuleBase<GetLastMessagesRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private EsInteractor m_esInteractor;

        public override bool IsFinalHandler => true;

        public GetLastMessagesModule(EsInteractor esInteractor, JwtHelper jwtHelper)
            : base (Routes.GET_LAST_MESSAGES, jwtHelper)
        {
            m_esInteractor = esInteractor;
        }

        protected override async Task OnRequest(IHttpContext context, GetLastMessagesRequest request, IEnumerable<Claim> claims)
        {
            m_logger.Trace($"Retrieving messages of chat {request.ChatId}");

            MessagesResponse messagesResponse = m_esInteractor.GetLastMessagesOfChat(request.ChatId);

            await SendResponse(context, HttpStatusCode.OK, JsonConvert.SerializeObject(messagesResponse));
        }
    }
}
