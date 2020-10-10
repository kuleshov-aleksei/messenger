using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.MessengerServer.HttpModules.GetLastMessages
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
