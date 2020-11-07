using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.UserServer.ChangeSurname
{
    internal sealed class ChangeSurameRequest : RequestBase
    {
        [JsonProperty("new_surname")]
        public string NewSurname { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(NewSurname))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewSurname))
            {
                return false;
            }

            return true;
        }
    }
}
