using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using Messenger.Common.Settings;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.UserServer.ChangeSurname
{
    internal sealed class ChangeSurnameModule : ModuleBase<ChangeSurameRequest>
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        public ChangeSurnameModule(JwtHelper jwtHelper)
            : base(Routes.CHANGE_SURNAME, jwtHelper)
        {
            
        }

        protected override async Task OnRequest(IHttpContext context, ChangeSurameRequest request, int userId)
        {
            m_logger.Info($"Got request for changing surname from user {userId}. New surname = \"{request.NewSurname}\"");

            string sql = @" UPDATE `user`
                            SET `surname` = @p_new_surname
                            WHERE `id` = @p_user_id;";

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("p_new_surname", request.NewSurname);
            parameters.Add("p_user_id", userId);

            await GlobalSettings.Instance.Database.ExecuteSqlAsync(sql, null, parameters);

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
