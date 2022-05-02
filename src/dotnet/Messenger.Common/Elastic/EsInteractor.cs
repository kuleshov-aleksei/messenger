using Messenger.Common.Elastic.Models;
using Messenger.Common.Models;
using Messenger.Common.Settings;
using Messenger.Common.Tools;
using Nest;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.Common.Elastic
{
    public class EsInteractor
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private readonly ElasticClient m_elasticClient;
        private readonly IdGenerator m_idGenerator;

        private Fields m_includeFields = new Field[] {
            GlobalSettings.EsFieldChatId,
            GlobalSettings.EsFieldText,
            GlobalSettings.EsFieldUserId,
            GlobalSettings.EsFieldMessageTime,
            GlobalSettings.EsFieldMessageImageUrlOriginal,
            GlobalSettings.EsFieldMessageAttachmentUrl,
        };

        public EsInteractor(ElasticClient elasticClient, IdGenerator idGenerator)
        {
            m_elasticClient = elasticClient;
            m_idGenerator = idGenerator;
        }

        public async Task<ChatMessages> GetLastMessagesOfChatAsync(int chatId)
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

            return await GetMessagesAsync(searchRequest, chatId);
        }

        public async Task<ChatMessages> GetMessagesBeforeAsync(long unixTime, int chatId)
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
                            LessThan = unixTime
                        }
                    },
                    MinimumShouldMatch = 2
                }
            };

            return await GetMessagesAsync(searchRequest, chatId);
        }

        public async Task<ChatMessages> GetMessagesFromAsync(long unixTime, int chatId)
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

            return await GetMessagesAsync(searchRequest, chatId);
        }

        private async Task<ChatMessages> GetMessagesAsync(SearchRequest searchRequest, int chatId)
        {
#if DEBUG
            string queryString = ESClient.RequestToReadableString(m_elasticClient, searchRequest);
#endif

            ISearchResponse<ElasticDocument> searchResponse = await m_elasticClient.SearchAsync<ElasticDocument>(searchRequest);

            if (!searchResponse.IsValid)
            {
                return null;
            }

            if (searchResponse.Hits.Count == 0)
            {
                return null;
            }

            ChatMessages response = new ChatMessages();
            response.ChatId = chatId;

            Dictionary<int, User> users = User.GetUsersForChat(chatId);

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
                    ChatId = chatId,
                    AttachmentUrl = document.attachment_url,
                    ImageUrl = document.image_url_original,
                };

                response.Messages.Add(message);
            };

            return response;
        }

        public async Task<bool> PutMessageAsync(EsMessage message)
        {
            string messageId = $"{m_idGenerator.GenerateUniqueId()}";

            Dictionary<string, object> requestDoc = new Dictionary<string, object>()
            {
                { GlobalSettings.EsFieldChatId, message.ChatId },
                { GlobalSettings.EsFieldMessageId, messageId },
                { GlobalSettings.EsFieldText, message.Message },
                { GlobalSettings.EsFieldUserId, message.UserId },
                { GlobalSettings.EsFieldMessageTime, message.MessageTime },
                { GlobalSettings.EsFieldMessageImageUrlOriginal, message.ImageUrlOriginal },
                { GlobalSettings.EsFieldMessageAttachmentUrl, message.AttachmentUrl },
            };

            IUpdateRequest<object, object> updateRequest = new UpdateRequest<object, object>(GlobalSettings.EsIndexName, messageId)
            {
                Doc = requestDoc,
                DocAsUpsert = true,
                RetryOnConflict = 3
            };

            IUpdateResponse<object> response = await m_elasticClient.UpdateAsync(updateRequest);
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
