using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using Messenger.Common.Settings;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.UserServer.ChangePassword
{
    internal sealed class ChangePasswordModule : ModuleBase<ChangePasswordRequest>
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public override bool IsFinalHandler => true;

        public ChangePasswordModule(JwtHelper jwtHelper)
            : base(Routes.CHANGE_PASSWORD, jwtHelper)
        {
            
        }

        protected override async Task OnRequest(IHttpContext context, ChangePasswordRequest request, int userId)
        {
            m_logger.Info($"Got request for changing password from user {userId}");

            List<Tuple<string, object, ParameterDirection, MySqlDbType>> parameters = new List<Tuple<string, object, ParameterDirection, MySqlDbType>>();

            parameters.Add(new Tuple<string, object, ParameterDirection, MySqlDbType>("p_user_id", userId, ParameterDirection.Input, MySqlDbType.Int32));
            parameters.Add(new Tuple<string, object, ParameterDirection, MySqlDbType>("p_current_password", request.CurrentPassword, ParameterDirection.Input, MySqlDbType.VarChar));
            parameters.Add(new Tuple<string, object, ParameterDirection, MySqlDbType>("p_new_password", request.NewPassword, ParameterDirection.Input, MySqlDbType.VarChar));

            try
            {
                GlobalSettings.Instance.Database.ExecuteProcedure("p_change_password", parameters, out Dictionary<string, object> returnValue);
            }
            catch (MySqlException e)
            {
                m_logger.Error($"Failed to execute procedure p_change_password: {e.Message}. {e.StackTrace}");

                if (e.Message == "invalid password")
                {
                    await SendResponse(context, HttpStatusCode.BadRequest, new ServerError("Неправильный пароль"));
                    return;
                }
                else
                {
                    await SendResponse(context, HttpStatusCode.BadRequest, new ServerError("Invalid request"));
                    return;
                }
            }

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
