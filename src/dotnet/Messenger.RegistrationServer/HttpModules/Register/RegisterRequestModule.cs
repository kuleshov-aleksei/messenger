using EmbedIO;
using Messenger.Common;
using Messenger.Common.Http;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.RegistrationService.HttpModules.Register
{
    public class RegisterRequestModule : ModuleBase<RegisterRequest>
    {
        public override bool IsFinalHandler => true;

        public RegisterRequestModule()
            : base (Routes.REGISTER, null)
        {
            base.NeedAuthorization = false;
        }

        protected override async Task OnRequest(IHttpContext context, RegisterRequest request, IEnumerable<Claim> claims)
        {
            bool userExists = false;

            GlobalSettings.Instance.Database.ExecuteSql(
                @"SELECT *
                FROM `user`
                WHERE `user`.`email` = @email
                OR `user`.`username` = @username",
                reader =>
                {
                    userExists = true;
                },
                new Dictionary<string, object>
                {
                    { "email", request.Email },
                    { "username", request.UserName }
                }
            );

            if (userExists)
            {
                await SendResponse(context, HttpStatusCode.Conflict, new ServerError("User with this username or email already exists"));
                return;
            }

            await GlobalSettings.Instance.Database.ExecuteProcedureAsync(
                "p_create_user", 
                new Dictionary<string, object>
                {
                    { "p_username", request.UserName },
                    { "p_name", request.Name },
                    { "p_surname", request.Surname },
                    { "p_email", request.Email },
                    { "p_password", request.Password },
                }
            );

            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
