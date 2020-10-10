using Messenger.Common.Http;
using Newtonsoft.Json;
using MySql.Common;
using Messenger.Common;

namespace Messenger.MessengerServer.HttpModules.PutMessage
{
    internal class PutMessageRequest : RequestBase
    {
        [JsonProperty("author_id")]
        public int AuthorId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        public override bool Validate()
        {
            if (AuthorId <= 0)
            {
                return false;
            }

            if (string.IsNullOrEmpty(Message))
            {
                return false;
            }

            if (ChatId <= 0)
            {
                return false;
            }

            bool invalid = false;
            GlobalSettings.Instance.Database.ExecuteSql(
                $@"SELECT COUNT(*) AS 'count'
                FROM `v_chat_members`
                WHERE `chat_id` = {ChatId}
                AND `user_id` = {AuthorId}",
                reader =>
                {
                    int? count = reader.GetInt32("count");
                    if (!count.HasValue)
                    {
                        invalid = true;
                        return;
                    }

                    if (count.Value == 0)
                    {
                        invalid = true;
                        return;
                    }
                });

            if (invalid)
            {
                return false;
            }

            return true;
        }
    }
}
