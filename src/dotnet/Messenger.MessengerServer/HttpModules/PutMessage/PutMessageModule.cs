using EmbedIO;
using Messenger.Common;
using Messenger.Common.Http;
using Messenger.Common.JWT;
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
        private ElasticClient m_elasticClient;
        private IdGenerator m_idGenerator;

        public override bool IsFinalHandler => true;

        public PutMessageModule(ElasticClient elasticClient, IdGenerator idGenerator, JwtHelper jwtHelper)
            : base(Routes.PUT_MESSAGE, jwtHelper)
        {
            m_elasticClient = elasticClient;
            m_idGenerator = idGenerator;
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

            string messageId = $"{m_idGenerator.GenerateUniqueId()}";

            Dictionary<string, object> requestDoc = new Dictionary<string, object>()
            {
                { GlobalSettings.EsFieldChatId, request.ChatId },
                { GlobalSettings.EsFieldMessageId, messageId },
                { GlobalSettings.EsFieldText, request.Message },
                { GlobalSettings.EsFieldUserId, userId },
                { GlobalSettings.EsFieldMessageTime, UnixEpochTools.ToEpoch(DateTime.UtcNow) },
            };

            IUpdateRequest<object, object> updateRequest = new UpdateRequest<object, object>(GlobalSettings.EsIndexName, messageId)
            {
                Doc = requestDoc,
                DocAsUpsert = true,
                RetryOnConflict = 10
            };

            IUpdateResponse<object> response = m_elasticClient.Update<object, object>(updateRequest);
            if (response.ServerError != null)
            {
                m_logger.Error($"Error saving message {messageId}. Error: {response.ServerError}");
                await SendResponse(context, HttpStatusCode.InternalServerError);
                return;
            };

            if (!response.IsValid)
            {
                m_logger.Error($"Error saving message {messageId}. Error: {response.DebugInformation}");
                await SendResponse(context, HttpStatusCode.InternalServerError);
                return;
            }

            m_logger.Info($"Message {messageId} written");
            await SendResponse(context, HttpStatusCode.OK);
        }
    }
}
