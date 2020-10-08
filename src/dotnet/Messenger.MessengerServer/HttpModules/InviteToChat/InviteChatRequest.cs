using Newtonsoft.Json;

namespace Messenger.MessengerServer.HttpModules.InviteToChat
{
    public class InviteChatRequest
    {
        [JsonProperty("invited_user_id")]
        public int InvitedUserId { get; set; }

        [JsonProperty("added_by")]
        public int AddedBy { get; set; }

        [JsonProperty("chat_id")]
        public int ChatId { get; set; }
    }
}
