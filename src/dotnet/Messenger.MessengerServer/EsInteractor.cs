using Messenger.Common;
using Messenger.Common.Elastic;
using Messenger.Common.Tools;
using Nest;
using NLog;
using System;
using System.Collections.Generic;

namespace Messenger.MessengerServer
{
    public class EsInteractor
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private ElasticClient m_elasticClient;
        private IdGenerator m_idGenerator;

        private Fields m_includeFields = new Field[] {
            GlobalSettings.EsFieldChatId,
            GlobalSettings.EsFieldText,
            GlobalSettings.EsFieldUserId,
            GlobalSettings.EsFieldMessageTime,
        };

        public EsInteractor(ElasticClient elasticClient, IdGenerator idGenerator)
        {
            m_elasticClient = elasticClient;
            m_idGenerator = idGenerator;
        }

        public MessagesResponse GetLastMessagesOfChat(int chatId)
        {
            SearchRequest searchRequest = new SearchRequest(GlobalSettings.EsIndexName)
            {
                Size = GlobalSettings.EsInitialMessagesCount,
                Sort = new List<ISort>()
                {
                    new FieldSort
                    {
                        Field = GlobalSettings.EsFieldMessageTime,
                        Order = SortOrder.Descending
                    }
                },
                Source = new SourceFilter()
                {
                    Includes = m_includeFields
                },
                Query = new BoolQuery
                {
                    Should = new QueryContainer[]
                    {
                        new TermQuery
                        {
                            Field = GlobalSettings.EsFieldChatId,
                            Value = chatId
                        },
                    },
                    MinimumShouldMatch = 1
                }
            };

            return GetMessages(searchRequest, chatId);
        }

        public MessagesResponse GetMessagesFrom(long unixTime, int chatId)
        {
            SearchRequest searchRequest = new SearchRequest(GlobalSettings.EsIndexName)
            {
                Size = GlobalSettings.EsMessagesCount,
                Sort = new List<ISort>()
                {
                    new FieldSort
                    {
                        Field = GlobalSettings.EsFieldMessageTime,
                        Order = SortOrder.Descending
                    }
                },
                Source = new SourceFilter()
                {
                    Includes = m_includeFields
                },
                Query = new BoolQuery
                {
                    Should = new QueryContainer[]
                    {
                        new TermQuery
                        {
                            Field = GlobalSettings.EsFieldChatId,
                            Value = chatId
                        },
                        new LongRangeQuery
                        {
                            Field = GlobalSettings.EsFieldMessageTime,
                            GreaterThan = unixTime
                        }
                    },
                    MinimumShouldMatch = 2
                }
            };

            return GetMessages(searchRequest, chatId);
        }

        private MessagesResponse GetMessages(SearchRequest searchRequest, int chatId)
        {
#if DEBUG
            string queryString = ESClient.RequestToReadableString(m_elasticClient, searchRequest);
#endif

            ISearchResponse<ElasticDocument> searchResponse = m_elasticClient.Search<ElasticDocument>(searchRequest);

            if (searchResponse.Hits.Count == 0)
            {
                return null;
            }

            MessagesResponse response = new MessagesResponse();
            response.ChatId = chatId;

            Dictionary<int, User> users = User.GetUsers(chatId);

            foreach (IHit<ElasticDocument> hit in searchResponse.Hits)
            {
                ElasticDocument document = hit.Source;

                if (!users.ContainsKey(document.user_id))
                {
                    // This need to be fixed if user will be able to kick users from chats
                    m_logger.Info("Got message from absent user. Should not happen");
                    continue;
                }

                Message message = new Message
                {
                    Text = document.text,
                    UnixTime = document.message_time,
                    AuthorName = users[document.user_id].Name,
                    AuthorSurname = users[document.user_id].Surname,
                    AuthorImageLinkSmall = users[document.user_id].ImageSmall,
                    AuthorId = document.user_id,
                };

                response.Messages.Add(message);
            };

            return response;
        }

        internal bool PutMessage(EsMessage message)
        {
            string messageId = $"{m_idGenerator.GenerateUniqueId()}";

            Dictionary<string, object> requestDoc = new Dictionary<string, object>()
            {
                { GlobalSettings.EsFieldChatId, message.ChatId },
                { GlobalSettings.EsFieldMessageId, messageId },
                { GlobalSettings.EsFieldText, message.Message },
                { GlobalSettings.EsFieldUserId, message.UserId },
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
                return false;
            };

            if (!response.IsValid)
            {
                m_logger.Error($"Error saving message {messageId}. Error: {response.DebugInformation}");
                return false;
            }

            return true;
        }
    }
}
