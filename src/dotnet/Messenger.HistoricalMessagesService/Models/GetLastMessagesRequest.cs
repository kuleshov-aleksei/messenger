using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.HistoricalMessagesService.Models
{
    public class GetLastMessagesRequest : RequestBase
    {
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        public override bool Validate()
        {
            return ChatId > 0;
        }
    }
}
