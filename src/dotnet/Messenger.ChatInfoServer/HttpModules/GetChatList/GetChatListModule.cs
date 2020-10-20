using EmbedIO;
using Messenger.Common.Http;
using Newtonsoft.Json;
using NLog;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using MySql.Common;
using Messenger.Common;
using Messenger.Common.JWT;

namespace Messenger.ChatInfoServer.HttpModules.GetChatList
{
    internal class GetChatListModule : ModuleBase<GetChatListRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        public GetChatListModule(JwtHelper jwtHelper)
            : base(Routes.GET_CHAT_LIST, jwtHelper)
        {

        }

        protected override async Task OnRequest(IHttpContext context, GetChatListRequest request, int userId)
        {
            m_logger.Trace($"Loading chats of user {userId}");
            ChatList response = LoadChatsOfUser(userId);

            await SendResponse(context, HttpStatusCode.OK, JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        private ChatList LoadChatsOfUser(int userId)
        {
            ChatList chatList = new ChatList();

            string sql = $@"SELECT `chat`.`id` as 'id', `title`, `image_small`, `image_medium`, `user_id`
                        FROM `chat`
                        INNER JOIN `chat_members` ON `chat`.`id` = `chat_members`.`chat_id`
                        WHERE `chat_members`.`user_id` = {userId}";

            GlobalSettings.Instance.Database.ExecuteSql(sql, 
                reader =>
                {
                    Chat chat = new Chat
                    {
                        Id = reader.GetString("id"),
                        Title = reader.GetString("title"),
                        ImageMedium = reader.GetString("image_medium"),
                        ImageSmall = reader.GetString("image_small"),
                    };

                    chatList.Chats.Add(chat);
                }
            );

            return chatList;
        }
    }
}
