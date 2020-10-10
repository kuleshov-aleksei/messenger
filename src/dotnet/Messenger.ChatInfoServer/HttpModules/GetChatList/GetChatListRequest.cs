using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.ChatInfoServer.HttpModules.GetChatList
{
    public class GetChatListRequest : RequestBase
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        public override bool Validate()
        {
            if (UserId <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
