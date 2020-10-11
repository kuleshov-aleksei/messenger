using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.AuthServer.HttpModules.Auth
{
    public class AuthRequest : RequestBase
    {
        // Login can be an email or username
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Login))
            {
                return false;
            }

            if (string.IsNullOrEmpty(DeviceName))
            {
                return false;
            }

            return true;
        }
    }
}
