using EmbedIO;
using Messenger.Common;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using MySql.Data.MySqlClient;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.ChatInfoServer.HttpModules.InviteToChat
{
    public class InviteChatModuleByUsername : ModuleBase<InviteChatRequestByUsername>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        public InviteChatModuleByUsername(JwtHelper jwtHelper)
            : base(Routes.INVITE_TO_CHAT_USERNAME, jwtHelper)
        {

        }

        protected override async Task OnRequest(IHttpContext context, InviteChatRequestByUsername request, int userId)
        {
            m_logger.Info($"Inviting user {request.InvitedUsername} to chat {request.ChatId} by {userId}");

            try
            {
                await GlobalSettings.Instance.Database.ExecuteProcedureAsync("p_add_user_to_chat_by_name", new Dictionary<string, object>
                {
                    { "p_username", request.InvitedUsername },
                    { "p_added_by", userId },
                    { "p_chat_id", request.ChatId }
                });
            }
            catch (MySqlException e)
            {
                m_logger.Error($"Failed to execute procedure p_add_user_to_chat_by_name: {e.Message}. {e.StackTrace}");

                if (e.Message == "added_by is not a member of chat")
                {
                    await SendResponse(context, HttpStatusCode.Forbidden, new ServerError("Текущий пользователь не может пригласить нового пользователя"));
                    return;
                }
                else if (e.Message == "user not found")
                {
                    await SendResponse(context, HttpStatusCode.BadRequest, new ServerError("Пользователь не найден"));
                    return;
                }
                else
                {
                    await SendResponse(context, HttpStatusCode.BadRequest, new ServerError("Invalid request"));
                    return;
                }
            }

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
