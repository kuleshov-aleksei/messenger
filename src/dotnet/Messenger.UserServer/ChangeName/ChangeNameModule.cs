using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using Messenger.Common.Settings;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.UserServer.ChangeName
{
    internal sealed class ChangeNameModule : ModuleBase<ChangeNameRequest>
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        public ChangeNameModule(JwtHelper jwtHelper)
            : base(Routes.CHANGE_NAME, jwtHelper)
        {
            
        }

        protected override async Task OnRequest(IHttpContext context, ChangeNameRequest request, int userId)
        {
            m_logger.Info($"Got request for changing name from user {userId}. New name = \"{request.NewName}\"");

            string sql = @" UPDATE `user`
                            SET `name` = @p_new_name
                            WHERE `id` = @p_user_id;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("p_new_name", request.NewName);
            parameters.Add("p_user_id", userId);

            await GlobalSettings.Instance.Database.ExecuteSqlAsync(sql, null, parameters);

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
