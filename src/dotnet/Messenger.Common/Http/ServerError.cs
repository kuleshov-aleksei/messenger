using Newtonsoft.Json;

namespace Messenger.Common.Http
{
    public class ServerError
    {
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        public ServerError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
