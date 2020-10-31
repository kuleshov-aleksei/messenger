using EmbedIO;
using Messenger.AuthServer.HttpModules.Auth;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using NLog;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.AuthServer.HttpModules.RefreshAccessToken
{
    public class RefreshAccessTokenModule : ModuleBase<RefreshRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        public override bool IsFinalHandler => true;

        public JwtHelper m_jwtHelper;

        public RefreshAccessTokenModule(JwtHelper jwtHelper)
            : base (Routes.REFRESH, null)
        {
            m_jwtHelper = jwtHelper;
            base.NeedAuthorization = false;
        }

        protected override async Task OnRequest(IHttpContext context, RefreshRequest request, int userId)
        {
            m_logger.Info($"Refreshing token of user {userId}");

            string accessToken = m_jwtHelper.CreateAccessJWT(request.RefreshToken, out string newRefreshToken);

            if (accessToken == null || newRefreshToken == null)
            {
                await SendResponse(context, HttpStatusCode.Unauthorized);
            }

            AuthResponse authResponse = new AuthResponse
            {
                RefreshToken = newRefreshToken,
                AccessToken = accessToken
            };

            await SendResponse(context, HttpStatusCode.OK, authResponse);
        }
    }
}
