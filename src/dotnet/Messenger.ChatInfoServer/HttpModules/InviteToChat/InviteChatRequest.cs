using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.ChatInfoServer.HttpModules.InviteToChat
{
    public class InviteChatRequest : RequestBase
    {
        [JsonProperty("invited_user_id")]
        public int InvitedUserId { get; set; }

        [JsonProperty("added_by")]
        public int AddedBy { get; set; }

        [JsonProperty("chat_id")]
        public int ChatId { get; set; }

        public override bool Validate()
        {
            if (AddedBy <= 0)
            {
                return false;
            }

            if (InvitedUserId <= 0)
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
