using Newtonsoft.Json;

namespace Messenger.Common.Http.Ping
{
    internal class PingResponse
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
