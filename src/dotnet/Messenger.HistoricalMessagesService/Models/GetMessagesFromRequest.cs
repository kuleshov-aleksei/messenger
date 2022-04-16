using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.HistoricalMessagesService.Models
{
    public class GetMessagesFromRequest
    {
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        [JsonProperty("time_from")]
        public long TimeFrom { get; set; }
    }
}
