using EmbedIO;
using Messenger.Common;
using Messenger.Common.Elastic;
using Messenger.Common.Http;
using Nest;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.HttpModules.GetLastMessages
{
    public class GetLastMessagesModule : ModuleBase<GetLastMessagesRequest>
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private ElasticClient m_elasticClient;

        public override bool IsFinalHandler => true;

        private Fields m_includeFields = new Field[] {
            GlobalSettings.EsFieldChatId,
            GlobalSettings.EsFieldText,
            GlobalSettings.EsFieldUserId,
            GlobalSettings.EsFieldMessageTime,
        };


        public GetLastMessagesModule(ElasticClient elasticClient)
            :base (Routes.GET_LAST_MESSAGES)
        {
            m_elasticClient = elasticClient;
        }

        protected override async Task OnRequest(IHttpContext context, GetLastMessagesRequest request)
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
                            Value = request.ChatId
                        },
                    },
                    MinimumShouldMatch = 1
                }
            };

#if DEBUG
            string queryString = ESClient.RequestToReadableString(m_elasticClient, searchRequest);
#endif

            ISearchResponse<ElasticDocument> searchResponse = m_elasticClient.Search<ElasticDocument>(searchRequest);

            if (searchResponse.Hits.Count == 0)
            {
                m_logger.Trace("No messages found");
            }

            GetLastMessagesResponse response = new GetLastMessagesResponse();
            response.ChatId = request.ChatId;

            Dictionary<int, User> users = User.GetUsers(request.ChatId);

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

            await SendResponse(context, HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }
    }
}
