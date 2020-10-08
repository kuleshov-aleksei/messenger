using EmbedIO;
using Messenger.Common.Http;
using Messenger.MessengerServer.HttpModules.GetChatList;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.HttpModules.CreateChat
{
    internal class CreateChatModule : ModuleBase<CreateChatRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        public override bool IsFinalHandler => true;

        public CreateChatModule()
            : base (Routes.CREATE_CHAT)
        {

        }

        protected override async Task OnRequest(IHttpContext context, CreateChatRequest request)
        {
            if (request.UserId == 0 || string.IsNullOrEmpty(request.Title))
            {
                await SendResponse(context, HttpStatusCode.BadRequest);
                return;
            }

            m_logger.Info($"Creating chat \"{request.Title}\"");

            await GlobalSettings.Instance.Database.ExecuteProcedureAsync("p_create_chat", new Dictionary<string, object>
            {
                { "title", request.Title },
                { "user_id", request.UserId }
            });

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
