using Newtonsoft.Json;

namespace Messenger.Common.Http
{
    public class ServerResponse
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        public ServerResponse(string text)
        {
            Text = text;
        }
    }
}
