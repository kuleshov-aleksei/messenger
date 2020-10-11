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
        public string ToJson(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public abstract bool Validate();

        public bool CheckAuthorization(JwtHelper jwtHelper, ICookieCollection cookies, out IEnumerable<Claim> claims)
        {
            Cookie cookie = cookies.First(x => x.Name == JwtHelper.AccessTokenName);
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                claims = null;
                return false;
            }

            string accessToken = cookie.Value;

            return jwtHelper.Validate(accessToken, out claims);
        }
    }
}
