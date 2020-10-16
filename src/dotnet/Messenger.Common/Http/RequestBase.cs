using EmbedIO;
using Messenger.Common.JWT;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Messenger.Common.Http
{
    public abstract class RequestBase : IRequest
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public string ToJson(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public abstract bool Validate();

        public bool CheckAuthorization(JwtHelper jwtHelper, RequestBase requestBase, out int userId)
        {
            if (requestBase == null || string.IsNullOrEmpty(requestBase.AccessToken))
            {
                userId = 0;
                return false;
            }

            return jwtHelper.Validate(requestBase.AccessToken, out userId);
        }
    }
}
