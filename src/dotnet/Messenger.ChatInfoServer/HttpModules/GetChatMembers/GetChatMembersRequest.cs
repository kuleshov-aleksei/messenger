using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.ChatInfoServer.HttpModules.GetChatMembers
{
    public class GetChatMembersRequest : RequestBase
    {
        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        public override bool Validate()
        {
            if (ChatId <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
