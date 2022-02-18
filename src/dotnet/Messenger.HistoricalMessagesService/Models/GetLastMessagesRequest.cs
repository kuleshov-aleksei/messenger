using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.HistoricalMessagesService.Models
{
    public class GetLastMessagesRequest
    {
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }
    }
}
