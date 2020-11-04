using EmbedIO;
using Messenger.Common;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using System.Threading.Tasks;
using MySql.Common;
using Nest;
using System.Net;
using System.Collections.Generic;

namespace Messenger.UserServer.GetInfo
{
    internal sealed class GetInfoModule : ModuleBase<GetInfoRequest>
    {
        internal GetInfoModule(JwtHelper jwtHelper)
            : base(Routes.GET_INFO, jwtHelper)
        {

        }

        public override bool IsFinalHandler => true;

        protected override async Task OnRequest(IHttpContext context, GetInfoRequest request, int userId)
        {
            string sql = 
$@"SELECT 
	`user`.`username`,
	`user`.`name`,
	`user`.`surname`,
	`user`.`email`,
	`user`.`image_small`,
	`user`.`image_medium`,
	`user`.`image_large`,
	`date_assigned`,
	`assigned_user`.`username` AS 'assigned_username',
	`assigned_user`.`name` AS 'assigned_name',
	`assigned_user`.`surname` AS 'assigned_surname',
	`role` AS 'role_title',
	`description` AS 'role_description'
FROM `user`
INNER JOIN `user_roles` ON `user`.`id` = `user_roles`.`user_id`
INNER JOIN `user` AS `assigned_user` ON `assigned_user`.`id` = `user_roles`.`assigned_by`
INNER JOIN `user_role` ON `user_roles`.`role_id` = `user_role`.`id`
WHERE `user`.`id` = {userId}";

            GetInfoResponse response = new GetInfoResponse();
            response.Roles = new List<Role>();

            GlobalSettings.Instance.Database.ExecuteSql(sql,
                reader =>
                {
                    response.Username = reader.GetString("username");
                    response.Surname = reader.GetString("surname");
                    response.Name = reader.GetString("name");
                    response.Email = reader.GetString("email");
                    response.ImageSmall = reader.GetString("image_small");
                    response.ImageMedium = reader.GetString("image_medium");
                    response.ImageLarge = reader.GetString("image_large");

                    Role role = new Role
                    {
                        Title = reader.GetString("role_title"),
                        Description = reader.GetString("role_description"),
                        DateAssigned = reader.GetDateTime("date_assigned").Value,
                        AssignedByName = reader.GetString("assigned_name"),
                        AssignedByUsername = reader.GetString("assigned_username"),
                        AssignedBySurname = reader.GetString("assigned_surname"),
                    };

                    response.Roles.Add(role);
                });

            await SendResponse(context, HttpStatusCode.OK, response);
        }
    }
}
