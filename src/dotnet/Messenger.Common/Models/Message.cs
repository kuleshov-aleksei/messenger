using Newtonsoft.Json;

namespace Messenger.Common.Models
{
    public class Message
    {
        [JsonProperty("unix_time")]
        public long UnixTime { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_surname")]
        public string AuthorSurname { get; set; }

        [JsonProperty("author_image_link_small")]
        public string AuthorImageLinkSmall { get; set; }

        [JsonProperty("author_id")]
        public int AuthorId { get; set; }

        [JsonProperty("chat_id")]
        public int ChatId { get; set; }
    }
}
