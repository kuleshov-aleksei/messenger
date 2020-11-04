using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.ChatInfoServer.HttpModules.InviteToChat
{
    public class InviteChatRequestByUsername : RequestBase
    {
        [JsonProperty("invited_username")]
        public string InvitedUsername { get; set; }


        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        public override bool Validate()
        {
            if (string.IsNullOrEmpty(InvitedUsername))
            {
                return false;
            }

            if (ChatId <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
