using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.UserServer.ChangePassword
{
    internal sealed class ChangePasswordRequest : RequestBase
    {
        [JsonProperty("current_password")]
        public string CurrentPassword { get; set; }

        [JsonProperty("new_password")]
        public string NewPassword { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(CurrentPassword) || string.IsNullOrWhiteSpace(CurrentPassword))
            {
                return false;
            }

            if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrWhiteSpace(NewPassword))
            {
                return false;
            }

            return true;
        }
    }
}
