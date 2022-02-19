using Newtonsoft.Json;

namespace Messenger.InstantMessagingService.Models
{
    public class SendMessageRequest
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("chat_id")]
        public int ChatId { get; set; }
    }
}
