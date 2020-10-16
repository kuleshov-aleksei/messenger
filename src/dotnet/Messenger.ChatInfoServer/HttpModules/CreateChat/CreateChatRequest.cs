using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.ChatInfoServer.HttpModules.GetChatList
{
    public class CreateChatRequest : RequestBase
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return false;
            }

            return true;
        }
    }
}
