using EmbedIO;
using Messenger.Common;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using MySql.Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.AuthServer.HttpModules.Auth
{
    public class AuthModule : ModuleBase<AuthRequest>
    {
        public override bool IsFinalHandler => true;
        public JwtHelper m_jwtHelper;

        public AuthModule()
            : base(Routes.AUTH)
        {
            AssemblyName currentAssembly = Assembly.GetExecutingAssembly().GetName();
            string issuer = $"{currentAssembly.Name}.{currentAssembly.Version.Major}.{currentAssembly.Version.Minor}.{currentAssembly.Version.Build}";
            m_jwtHelper = new JwtHelper(issuer, "jwt_secret.secret");
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

            int userId = GetUserId(request.Login);

            string refreshToken = m_jwtHelper.CreateRefreshJWT(request.DeviceName, userId);
            string accessToken = m_jwtHelper.CreateAccessJWT(refreshToken);

            context.Response.SetCookie(new Cookie("access_token", accessToken));
            context.Response.SetCookie(new Cookie("refresh_token", refreshToken));

            await SendResponse(context, HttpStatusCode.OK);
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

        private int GetUserId(string login)
        {
            int userId = 0;

            GlobalSettings.Instance.Database.ExecuteSql(
                @"SELECT `user`.`id` as id
		        FROM `user`
		        WHERE `email` = @p_login
                OR `username` = @p_login",
                reader =>
                {
                    userId = reader.GetInt32("id").Value;
                },
                new Dictionary<string, object>
                {
                    { "p_login", login }
                }
            );

            return userId;
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
