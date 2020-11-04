using Messenger.Common.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Messenger.UserServer.GetInfo
{
    internal sealed class GetInfoResponse : ResponseBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("roles")]
        public List<Role> Roles { get; set; }

        [JsonProperty("image_large")]
        public string ImageLarge { get; set; }

        [JsonProperty("image_medium")]
        public string ImageMedium { get; set; }

        [JsonProperty("image_small")]
        public string ImageSmall { get; set; }
    }
}
