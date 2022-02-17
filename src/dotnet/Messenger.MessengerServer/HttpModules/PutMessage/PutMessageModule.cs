using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using Messenger.Common.Settings;
using Messenger.Common.Tools;
using MySql.Common;
using Nest;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.HttpModules.PutMessage
{
    internal class PutMessageModule : ModuleBase<PutMessageRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private EsInteractor m_esInteractor;

        public override bool IsFinalHandler => true;

        public PutMessageModule(EsInteractor esInteractor, JwtHelper jwtHelper)
            : base(Routes.PUT_MESSAGE, jwtHelper)
        {
            m_esInteractor = esInteractor;
        }

        // TODO: Add user validation
        protected override async Task OnRequest(IHttpContext context, PutMessageRequest request, int userId)
        {
            m_logger.Info($"Writing message from user {userId} to chat {request.ChatId}");

            bool unathorized = false;
            GlobalSettings.Instance.Database.ExecuteSql(
                $@"SELECT COUNT(*) AS 'count'
                FROM `v_chat_members`
                WHERE `chat_id` = {request.ChatId}
                AND `user_id` = {userId}",
                reader =>
                {
                    int? count = reader.GetInt32("count");
                    if (!count.HasValue)
                    {
                        unathorized = true;
                        return;
                    }

                    if (count.Value == 0)
                    {
                        unathorized = true;
                        return;
                    }
                });

            if (unathorized)
            {
                await SendResponse(context, HttpStatusCode.Unauthorized);
                return;
            }

            EsMessage esMessage = new EsMessage
            {
                ChatId = request.ChatId,
                Message = request.Message,
                UserId = userId
            };

            bool putResult = m_esInteractor.PutMessage(esMessage);
            if (!putResult)
            {
                await SendResponse(context, HttpStatusCode.InternalServerError);
            }
            else
            {
                m_logger.Info($"Message written");
                await SendResponse(context, HttpStatusCode.OK);
            }
        }
    }
}
