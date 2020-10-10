using Messenger.Common;
using Messenger.Common.Elastic;
using Nest;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.MessengerServer
{
    public class EsInteractor
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private ElasticClient m_elasticClient;

        private Fields m_includeFields = new Field[] {
            GlobalSettings.EsFieldChatId,
            GlobalSettings.EsFieldText,
            GlobalSettings.EsFieldUserId,
            GlobalSettings.EsFieldMessageTime,
        };

        public EsInteractor(ElasticClient elasticClient)
        {
            m_elasticClient = elasticClient;
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
                            LessThan = unixTime
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
                m_logger.Trace("No messages found");
            }

            MessagesResponse response = new MessagesResponse();
            response.ChatId = chatId;

            Dictionary<int, User> users = User.GetUsers(chatId);

            foreach (IHit<ElasticDocument> hit in searchResponse.Hits)
            {
                ElasticDocument document = hit.Source;

                Message message = new Message
                {
                    Text = document.text,
                    UnixTime = document.message_time,
                    AuthorName = users[document.user_id].Name,
                    AuthorSurname = users[document.user_id].Surname,
                    AuthorImageLinkSmall = users[document.user_id].ImageSmall,
                };

                response.Messages.Add(message);
            };

            return response;
        }
    }
}
