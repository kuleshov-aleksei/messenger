using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.AuthServer.HttpModules.RefreshAccessToken
{
    public class RefreshAccessTokenModule : ModuleBase<RequestBase>
    {
        public override bool IsFinalHandler => true;

        public JwtHelper m_jwtHelper;

        public RefreshAccessTokenModule(JwtHelper jwtHelper)
            : base (Routes.REFRESH)
        {
            m_jwtHelper = jwtHelper;
        }

        protected override async Task OnRequest(IHttpContext context, RequestBase request)
        {
            Cookie cookie = context.Request.Cookies.First(x => x.Name == JwtHelper.RefreshTokenName);
            if (cookie == null)
            {
                await SendResponse(context, HttpStatusCode.BadRequest, new ServerError("assess token is empty"));
                return;
            }

            string refreshToken = cookie.Value;

            string accessToken = m_jwtHelper.CreateAccessJWT(refreshToken, out string newRefreshToken);

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
