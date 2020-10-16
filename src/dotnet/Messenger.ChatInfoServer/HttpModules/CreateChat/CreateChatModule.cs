using EmbedIO;
using Messenger.Common.Http;
using Messenger.ChatInfoServer.HttpModules.GetChatList;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Messenger.Common;
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

        protected override async Task OnRequest(IHttpContext context, CreateChatRequest request, int userId)
        {
            m_logger.Info($"Creating chat \"{request.Title}\"");

            await GlobalSettings.Instance.Database.ExecuteProcedureAsync("p_create_chat", new Dictionary<string, object>
            {
                { "title", request.Title },
                { "user_id", userId }
            });

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
