using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.MessengerServer.HttpModules.GetChatMembers
{
    public class GetChatMembersResponse
    {
        [JsonProperty("chat_members")]
        public List<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();
    }

    public class ChatMember
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("joined_at")]
        public DateTime? JoinedAt { get; set; }

        [JsonProperty("invited_by_name")]
        public string InvitedByName { get; set; }

        [JsonProperty("invited_by_surname")]
        public string InvitedBySurname { get; set; }
    }
}
