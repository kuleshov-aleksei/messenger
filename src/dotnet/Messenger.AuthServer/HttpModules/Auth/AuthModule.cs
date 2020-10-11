using EmbedIO;
using Messenger.Common;
using Messenger.Common.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.AuthServer.HttpModules.Auth
{
    public class AuthModule : ModuleBase<AuthRequest>
    {
        public override bool IsFinalHandler => true;

        public AuthModule()
            : base(Routes.AUTH)
        {

        }

        protected override async Task OnRequest(IHttpContext context, AuthRequest request)
        {
            bool usernameSucceed = AuthenticateUsername(request.Login, request.Password);
            bool emailSucceed = false;

            if (!usernameSucceed)
            {
                emailSucceed = AuthenticateEmail(request.Login, request.Password);
            }

            if (!usernameSucceed && !emailSucceed)
            {
                await SendResponse(context, HttpStatusCode.Forbidden);
                return;
            }
        }

        private bool AuthenticateUsername(string username, string password)
        {
            List<Tuple<string, object, ParameterDirection, MySqlDbType>> parameters = new List<Tuple<string, object, ParameterDirection, MySqlDbType>>();

            parameters.Add(new Tuple<string, object, ParameterDirection, MySqlDbType>("p_username", username, ParameterDirection.Input, MySqlDbType.VarChar));
            parameters.Add(new Tuple<string, object, ParameterDirection, MySqlDbType>("p_password", password, ParameterDirection.Input, MySqlDbType.VarChar));
            parameters.Add(new Tuple<string, object, ParameterDirection, MySqlDbType>("p_out_valid", null, ParameterDirection.Output, MySqlDbType.Int32));

            GlobalSettings.Instance.Database.ExecuteProcedure("p_valid_password_username", parameters, out Dictionary<string, object> returnValue);

            return (int)returnValue["p_out_valid"] > 0;
        }

        private bool AuthenticateEmail(string email, string password)
        {
            List<Tuple<string, object, ParameterDirection, MySqlDbType>> parameters = new List<Tuple<string, object, ParameterDirection, MySqlDbType>>();

            parameters.Add(new Tuple<string, object, ParameterDirection, MySqlDbType>("p_email", email, ParameterDirection.Input, MySqlDbType.VarChar));
            parameters.Add(new Tuple<string, object, ParameterDirection, MySqlDbType>("p_password", password, ParameterDirection.Input, MySqlDbType.VarChar));
            parameters.Add(new Tuple<string, object, ParameterDirection, MySqlDbType>("p_out_valid", null, ParameterDirection.Output, MySqlDbType.Int32));

            GlobalSettings.Instance.Database.ExecuteProcedure("p_valid_password_email", parameters, out Dictionary<string, object> returnValue);

            return (int)returnValue["p_out_valid"] > 0;
        }
    }
}
