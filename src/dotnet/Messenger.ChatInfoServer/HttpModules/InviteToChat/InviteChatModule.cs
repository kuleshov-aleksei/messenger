using EmbedIO;
using Messenger.Common.Http;
using MySql.Data.MySqlClient;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.ChatInfoServer.HttpModules.InviteToChat
{
    public class InviteChatModule : ModuleBase<InviteChatRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        public InviteChatModule()
            : base(Routes.INVITE_TO_CHAT)
        {

        }

        protected override async Task OnRequest(IHttpContext context, InviteChatRequest request)
        {
            if (request.AddedBy == 0 || request.InvitedUserId == 0 || request.ChatId == 0)
            {
                await SendResponse(context, HttpStatusCode.BadRequest);
                return;
            }

            m_logger.Info($"Inviting user {request.InvitedUserId} to chat {request.ChatId} by {request.AddedBy}");

            try
            {
                await GlobalSettings.Instance.Database.ExecuteProcedureAsync("p_add_user_to_chat", new Dictionary<string, object>
                {
                    { "new_user_id", request.InvitedUserId },
                    { "added_by", request.AddedBy },
                    { "chat_id", request.ChatId }
                });
            }
            catch (MySqlException e)
            {
                m_logger.Error(e);
                await SendResponse(context, HttpStatusCode.BadRequest, new ServerError("Invalid request"));
                return;
            }

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
