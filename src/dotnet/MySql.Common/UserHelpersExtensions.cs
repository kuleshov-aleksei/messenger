using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySql.Common
{
    public static class UserHelpersExtensions
    {
        public static async Task<List<int>> GetUserChatListAsync(this Database database, IRedisDatabase redisDatabase, int userId)
        {
            string userChatsKey = $"user_chats_{userId}";
            List<int> chats = await redisDatabase.GetAsync<List<int>>(userChatsKey);
            if (chats == null)
            {
                chats = GetChatListFromDb(database, userId);
                await redisDatabase.AddAsync(userChatsKey, chats, TimeSpan.FromMinutes(1));
            }

            return chats;
        }

        private static List<int> GetChatListFromDb(Database database, int userId)
        {
            List<int> result = new List<int>();
            database.ExecuteSql(
                $@"SELECT `chat_id`
                FROM `v_chat_members`
                WHERE `user_id` = {userId}",
                reader =>
                {
                    int chatId = reader.GetInt32("chat_id").Value;
                    result.Add(chatId);
                });

            return result;
        }
    }
}
