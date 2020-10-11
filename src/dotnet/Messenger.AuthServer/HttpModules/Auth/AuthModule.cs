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
using System.Threading.Tasks;

namespace Messenger.AuthServer.HttpModules.Auth
{
    public class AuthModule : ModuleBase<AuthRequest>
    {
        public override bool IsFinalHandler => true;
        public JwtHelper m_jwtHelper;

        public AuthModule(JwtHelper jwtHelper)
            : base(Routes.AUTH)
        {
            m_jwtHelper = jwtHelper;
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

            string tempRefreshToken = m_jwtHelper.CreateSession(request.DeviceName, userId);
            string accessToken = m_jwtHelper.CreateAccessJWT(tempRefreshToken, out string refreshToken);

            context.Response.SetCookie(new Cookie(JwtHelper.AccessTokenName, accessToken)
            {
                HttpOnly = true,
            });

            context.Response.SetCookie(new Cookie(JwtHelper.RefreshTokenName, refreshToken)
            {
                HttpOnly = true,
            });

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
