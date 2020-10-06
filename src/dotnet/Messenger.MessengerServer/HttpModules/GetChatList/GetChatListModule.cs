using EmbedIO;
using Messenger.Common.Http;
using Newtonsoft.Json;
using NLog;
using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using MySql.Common;

namespace Messenger.MessengerServer.HttpModules.GetChatList
{
    internal class GetChatListModule : ModuleBase
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        public GetChatListModule()
            :base(Routes.GET_CHAT_LIST)
        {

        }

        protected override async Task OnRequestAsync(IHttpContext context)
        {
            if (!TryGetRequestString(context.Request, out string requestString))
            {
                return;
            }

            GetChatListRequest request = null;
            try
            {
                request = JsonConvert.DeserializeObject<GetChatListRequest>(requestString);
            }
            catch (Exception e)
            {
                m_logger.Error(e, "Failed to parse request");
                await SendResponse(context, HttpStatusCode.OK, new ServerError("Invalid request"));
                return;
            }

            ChatList response = LoadChatsOfUser(request.UserId);

            await SendResponse(context, HttpStatusCode.OK, JsonConvert.SerializeObject(response, Formatting.Indented));
        }

        private ChatList LoadChatsOfUser(int userId)
        {
            ChatList chatList = new ChatList();

            string sql = $@"SELECT `title`, `image_small`, `image_medium`, `user_id`
                        FROM `chat`
                        INNER JOIN `chat_members` ON `chat`.`id` = `chat_members`.`chat_id`
                        WHERE `chat_members`.`user_id` = {userId}";

            GlobalSettings.Instance.Database.ExecuteSql(sql, 
                reader =>
                {
                    Chat chat = new Chat
                    {
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
