using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.ChatInfoServer.HttpModules.GetChatList
{
    public class CreateChatRequest : RequestBase
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        public override bool Validate()
        {
            if (UserId <= 0)
            {
                return false;
            }

            if (string.IsNullOrEmpty(Title))
            {
                return false;
            }

            return true;
        }
    }
}
