﻿using System.Collections.Generic;
using MySql.Common;
using Messenger.Common.Settings;

namespace Messenger.Common.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ImageSmall { get; set; }
        public string ImageMedium { get; set; }
        public string ImageLarge { get; set; }

        public static Dictionary<int, User> GetUsersForChat(int chatId)
        {
            string sql = $@"SELECT `user_id`, `name`, `surname`, `image_small`, `image_large`, `image_medium`
                        FROM `v_chat_members`
                        WHERE `chat_id` = {chatId}";

            Dictionary<int, User> users = new Dictionary<int, User>();

            GlobalSettings.Instance.Database.ExecuteSql(sql,
                reader =>
                {
                    int id = reader.GetInt32("user_id").Value;

                    User user = new User
                    {
                        Name = reader.GetString("name"),
                        Surname = reader.GetString("surname"),
                        ImageSmall = reader.GetString("image_small"),
                        ImageMedium = reader.GetString("image_medium"),
                        ImageLarge = reader.GetString("image_large"),
                    };

                    users.Add(id, user);
                }
            );

            return users;
        }
    }
}