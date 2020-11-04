using Newtonsoft.Json;
using System;

namespace Messenger.UserServer.GetInfo
{
    internal sealed class Role
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date_assigned")]
        public DateTime DateAssigned { get; set; }

        [JsonProperty("assigned_by_name")]
        public string AssignedByName { get; set; }

        [JsonProperty("assigned_by_surname")]
        public string AssignedBySurname { get; set; }

        [JsonProperty("assigned_by_username")]
        public string AssignedByUsername { get; set; }
    }
}
