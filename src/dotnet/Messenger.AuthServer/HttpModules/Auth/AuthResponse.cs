using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.AuthServer.HttpModules.Auth
{
    public class AuthResponse : ResponseBase
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
