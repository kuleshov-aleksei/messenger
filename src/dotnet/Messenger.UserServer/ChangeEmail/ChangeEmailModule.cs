using EmbedIO;
using Messenger.Common;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.UserServer.ChangeEmail
{
    internal sealed class ChangeEmailModule : ModuleBase<ChangeEmailRequest>
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        public ChangeEmailModule(JwtHelper jwtHelper)
            : base(Routes.CHANGE_EMAIL, jwtHelper)
        {
            
        }

        protected override async Task OnRequest(IHttpContext context, ChangeEmailRequest request, int userId)
        {
            m_logger.Info($"Got request for changing email from user {userId}. New email = \"{request.NewEmail}\"");

            string sql = @" UPDATE `user`
                            SET `email` = @p_new_email
                            WHERE `id` = @p_user_id;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("p_new_email", request.NewEmail);
            parameters.Add("p_user_id", userId);

            await GlobalSettings.Instance.Database.ExecuteSqlAsync(sql, null, parameters);

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
