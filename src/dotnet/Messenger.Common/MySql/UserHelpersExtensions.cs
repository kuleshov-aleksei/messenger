using Messenger.Common.Models;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.Common.MySql
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

        public static async Task<User> GetUser(this Database database, IRedisDatabase redisDatabase, int userId)
        {
            string userKey = $"user_{userId}";
            User user = await redisDatabase.GetAsync<User>(userKey);
            if (user == null)
            {
                user = await database.GetUserFromDb(userId);
                await redisDatabase.AddAsync(userKey, user, TimeSpan.FromMinutes(15));
            }

            return user;
        }

        private static async Task<User> GetUserFromDb(this Database database, int userId)
        {
            User user = new User();
            await database.ExecuteSqlAsync(
                $@"SELECT `name`, `surname`, `image_large`, `image_medium`, `image_small`
                FROM `user`
                WHERE `id` = {userId}",
                reader =>
                {
                    user.Name = reader.GetString("name");
                    user.Surname = reader.GetString("surname");
                    user.ImageSmall = reader.GetString("image_small");
                    user.ImageMedium = reader.GetString("image_medium");
                    user.ImageLarge = reader.GetString("image_large");
                }
            );

            return user;
        }
    }
}
