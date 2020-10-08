using Newtonsoft.Json;

namespace Messenger.Common.Http.Ping
{
    internal class PingRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
