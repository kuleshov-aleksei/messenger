using Messenger.Common;
using MySql.Common;
using NLog;
using System.Collections.Generic;

namespace Messenger.PictureService
{
    internal class User
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public int UserId { get; set; }
        public string ImageLargeLink { get; set; }
        public string ImageMediumLink { get; set; }
        public string ImageSmallLink { get; set; }

        internal void UpdateLinks()
        {
            if (UserId < 2)
            {
                m_logger.Warn("Invalid user id. Can't update user image links");
                return;
            }

            string sql = 
                $@"UPDATE `user`
                SET `image_large` = @new_image_large,
                    `image_medium` = @new_image_medium,
                    `image_small` = @new_image_small
                WHERE `id` = @user_id";

            GlobalSettings.Instance.Database.ExecuteSql(sql, parameters: new Dictionary<string, object>
            {
                { "new_image_small", ImageSmallLink },
                { "new_image_medium", ImageMediumLink },
                { "new_image_large", ImageLargeLink },
                { "user_id", UserId },
            });
        }

        internal static User ReadUserFromDb(int userId)
        {
            string sql = 
                @$"SELECT `id`, `image_large`, `image_medium`, `image_small`
                FROM `user`
                WHERE `id` = {userId}";

            User user = null;

            GlobalSettings.Instance.Database.ExecuteSql(sql,
                reader =>
                {
                    user = new User
                    {
                        UserId = reader.GetInt32("id").Value,
                        ImageLargeLink = reader.GetString("image_large"),
                        ImageMediumLink = reader.GetString("image_medium"),
                        ImageSmallLink = reader.GetString("image_small"),
                    };
                });

            return user;
        }
    }
}
