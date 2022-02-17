using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using Messenger.Common.Settings;
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

        public GetChatMembersModule(JwtHelper jwtHelper)
            : base(Routes.GET_CHAT_MEMBERS, jwtHelper)
        {

        }

        // TODO: Add user validation
        protected override async Task OnRequest(IHttpContext context, GetChatMembersRequest request, int userId)
        {
            GetChatMembersResponse response = LoadChatMembers(request.ChatId);

            await SendResponse(context, HttpStatusCode.OK, JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        private GetChatMembersResponse LoadChatMembers(int chatId)
        {
            GetChatMembersResponse response = new GetChatMembersResponse();
                 
            string sql = $@"SELECT `user_id`, `chat_id`, `name`, `surname`, `invited_by_name`, `invited_by_surname`, `joined_at`, `image_small`, `image_medium`, `image_large`, `username`
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
                        Id = reader.GetInt32("user_id").Value,
                        ImageSmall = reader.GetString("image_small"),
                        ImageMedium = reader.GetString("image_medium"),
                        ImageLarge = reader.GetString("image_large"),
                        UserName = reader.GetString("username"),
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
