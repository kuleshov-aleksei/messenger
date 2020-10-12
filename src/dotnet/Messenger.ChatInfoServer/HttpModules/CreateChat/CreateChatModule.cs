using EmbedIO;
using Messenger.Common.Http;
using Messenger.ChatInfoServer.HttpModules.GetChatList;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Messenger.Common;
using System.Security.Claims;
using Messenger.Common.JWT;

namespace Messenger.ChatInfoServer.HttpModules.CreateChat
{
    internal class CreateChatModule : ModuleBase<CreateChatRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        public override bool IsFinalHandler => true;

        public CreateChatModule(JwtHelper jwtHelper)
            : base (Routes.CREATE_CHAT, jwtHelper)
        {

        }

        protected override async Task OnRequest(IHttpContext context, CreateChatRequest request, IEnumerable<Claim> claims)
        {
            if (request.UserId == 0 || string.IsNullOrEmpty(request.Title))
            {
                await SendResponse(context, HttpStatusCode.BadRequest);
                return;
            }

            m_logger.Info($"Creating chat \"{request.Title}\"");

            int claimedUser = JwtHelper.GetUserId(claims);
            if (request.UserId != claimedUser)
            {
                m_logger.Info($"Trying to create chat from user {request.UserId}, but claimed user is {claimedUser}");
                await SendResponse(context, HttpStatusCode.Forbidden);
                return;
            }

            await GlobalSettings.Instance.Database.ExecuteProcedureAsync("p_create_chat", new Dictionary<string, object>
            {
                { "title", request.Title },
                { "user_id", request.UserId }
            });

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
