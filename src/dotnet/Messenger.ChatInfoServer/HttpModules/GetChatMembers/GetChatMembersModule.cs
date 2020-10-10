using EmbedIO;
using Messenger.Common;
using Messenger.Common.Http;
using MySql.Common;
using Newtonsoft.Json;
using NLog;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.ChatInfoServer.HttpModules.GetChatMembers
{
    internal class GetChatMembersModule : ModuleBase<GetChatMembersRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        public GetChatMembersModule()
            : base(Routes.GET_CHAT_MEMBERS)
        {

        }

        protected override async Task OnRequest(IHttpContext context, GetChatMembersRequest request)
        {
            GetChatMembersResponse response = LoadChatMembers(request.ChatId);

            await SendResponse(context, HttpStatusCode.OK, JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        private GetChatMembersResponse LoadChatMembers(int chatId)
        {
            GetChatMembersResponse response = new GetChatMembersResponse();
                 
            string sql = $@"SELECT `chat_id`, `name`, `surname`, `invited_by_name`, `invited_by_surname`, `joined_at`
                        FROM 
                        `v_chat_members`
                        WHERE `v_chat_members`.`chat_id` = {chatId}";

            GlobalSettings.Instance.Database.ExecuteSql(sql,
                reader =>
                {
                    ChatMember chatMember = new ChatMember
                    {
                        Name = reader.GetString("name"),
                        Surname = reader.GetString("surname"),
                        InvitedByName = reader.GetString("invited_by_name"),
                        InvitedBySurname = reader.GetString("invited_by_surname"),
                        JoinedAt = reader.GetDateTime("joined_at"),
                    };

                    if (chatMember.InvitedByName == "SYSTEM")
                    {
                        chatMember.InvitedByName = null;
                    }

                    if (chatMember.InvitedBySurname == "SYSTEM")
                    {
                        chatMember.InvitedBySurname = null;
                    }

                    response.ChatMembers.Add(chatMember);
                }
            );

            return response;
        }
    }
}
