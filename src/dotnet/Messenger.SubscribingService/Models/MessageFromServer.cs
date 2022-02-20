using Newtonsoft.Json;

namespace Messenger.SubscribingService.Models
{
    public class MessageFromServer
    {
        [JsonProperty("error_code")]
        public ErrorReasons Code { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("incoming_messages")]
        public MessageContainter MessageContainter { get; set; }
    }
}
