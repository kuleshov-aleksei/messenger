using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.UserServer.ChangeName
{
    internal sealed class ChangeNameRequest : RequestBase
    {
        [JsonProperty("new_name")]
        public string NewName { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(NewName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewName))
            {
                return false;
            }

            return true;
        }
    }
}
