using Messenger.Common.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Messenger.UserServer.ChangeEmail
{
    internal sealed class ChangeEmailRequest : RequestBase
    {
        [JsonProperty("new_email")]
        public string NewEmail { get; set; }

        private Regex m_regex = new Regex(@"\S+@\S+\.\S+", RegexOptions.Compiled);

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(NewEmail))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewEmail))
            {
                return false;
            }

            if (!m_regex.IsMatch(NewEmail))
            {
                return false;
            }

            return true;
        }
    }
}
