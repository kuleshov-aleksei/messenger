using Newtonsoft.Json;

namespace Messenger.MessengerServer.HttpModules.GetChatList
{
    public class CreateChatRequest
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
