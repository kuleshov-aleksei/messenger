using EmbedIO;
using Messenger.Common;
using Messenger.Common.Http;
using Messenger.Common.Tools;
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

        public PutMessageModule(ElasticClient elasticClient, IdGenerator idGenerator)
            :base(Routes.PUT_MESSAGE)
        {
            m_elasticClient = elasticClient;
            m_idGenerator = idGenerator;
        }

        protected override async Task OnRequest(IHttpContext context, PutMessageRequest request)
        {
            m_logger.Info($"Writing message from user {request.AuthorId} to chat {request.ChatId}");

            string messageId = $"{m_idGenerator.GenerateUniqueId()}";

            Dictionary<string, object> requestDoc = new Dictionary<string, object>()
            {
                { "chat_id", request.ChatId },
                { "message_id", messageId },
                { "text", request.Message },
                { "user_id", request.AuthorId },
                { "message_time", UnixEpochTools.ToEpoch(DateTime.UtcNow) },
            };

            IUpdateRequest<object, object> updateRequest = new UpdateRequest<object, object>(GlobalSettings.Instance.EsIndexName, messageId)
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
        }
    }
}
