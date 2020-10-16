using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.MessengerServer.HttpModules.PutMessage
{
    internal class PutMessageRequest : RequestBase
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return false;
            }

            if (ChatId <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
