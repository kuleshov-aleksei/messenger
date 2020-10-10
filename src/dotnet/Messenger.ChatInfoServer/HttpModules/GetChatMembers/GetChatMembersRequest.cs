using Newtonsoft.Json;

namespace Messenger.ChatInfoServer.HttpModules.GetChatMembers
{
    public class GetChatMembersRequest
    {
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }
    }
}
