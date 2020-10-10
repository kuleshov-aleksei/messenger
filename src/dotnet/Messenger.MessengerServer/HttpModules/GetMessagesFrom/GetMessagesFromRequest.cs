using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.MessengerServer.HttpModules.GetMessagesFrom
{
    public class GetMessagesFromRequest : RequestBase
    {
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        [JsonProperty("unix_time")]
        public long UnixTime { get; set; }

        public override bool Validate()
        {
            if (ChatId <= 0)
            {
                return false;
            }

            if (UnixTime <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
