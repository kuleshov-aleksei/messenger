using Newtonsoft.Json;

namespace Messenger.ChatInfoServer.HttpModules.GetChatList
{
    public class GetChatListRequest
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}
