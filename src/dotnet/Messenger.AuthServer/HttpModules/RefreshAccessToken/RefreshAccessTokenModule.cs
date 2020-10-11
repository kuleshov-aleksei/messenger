using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.AuthServer.HttpModules.RefreshAccessToken
{
    public class RefreshAccessTokenModule : ModuleBase<RequestBase>
    {
        public override bool IsFinalHandler => true;

        public JwtHelper m_jwtHelper;

        public RefreshAccessTokenModule(JwtHelper jwtHelper)
            : base (Routes.REFRESH, null)
        {
            m_jwtHelper = jwtHelper;
            base.NeedAuthorization = false;
        }

        protected override async Task OnRequest(IHttpContext context, RequestBase request, IEnumerable<Claim> claims)
        {
            Cookie cookie = context.Request.Cookies.First(x => x.Name == JwtHelper.RefreshTokenName);
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                await SendResponse(context, HttpStatusCode.Unauthorized, new ServerError("access token is empty"));
                return;
            }

            string refreshToken = cookie.Value;

            string accessToken = m_jwtHelper.CreateAccessJWT(refreshToken, out string newRefreshToken);

            if (accessToken == null || newRefreshToken == null)
            {
                await SendResponse(context, HttpStatusCode.Unauthorized);
            }

            context.Response.SetCookie(new Cookie(JwtHelper.AccessTokenName, accessToken)
            {
                HttpOnly = true,
            });

            context.Response.SetCookie(new Cookie(JwtHelper.RefreshTokenName, newRefreshToken)
            {
                HttpOnly = true,
            });

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
