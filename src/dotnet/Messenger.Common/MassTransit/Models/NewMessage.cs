using Newtonsoft.Json;

namespace Messenger.Common.MassTransit.Models
{
    public class NewMessage
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("message_time")]
        public long MessageTime { get; set; }

        [JsonProperty("message_attachment_url")]
        public string MessageAttachmentUrl { get; set; }

        [JsonProperty("message_image_url")]
        public string MessageImageUrl { get; set; }
    }
}
