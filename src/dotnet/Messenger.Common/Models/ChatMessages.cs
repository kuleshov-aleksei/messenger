using Newtonsoft.Json;
using System.Collections.Generic;

namespace Messenger.Common.Models
{
    public class ChatMessages
    {
        [JsonProperty("messages")]
        public List<Message> Messages { get; set; } = new List<Message>();

        [JsonProperty("chat_id")]
        public int ChatId { get; set; }
    }
}
