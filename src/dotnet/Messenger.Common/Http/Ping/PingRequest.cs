using Newtonsoft.Json;

namespace Messenger.Common.Http.Ping
{
    internal class PingRequest : RequestBase
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        public override bool Validate()
        {
            return true;
        }
    }
}
