using Elasticsearch.Net;
using Messenger.Common.Settings;
using Nest;
using System;
using System.IO;
using System.Text;

namespace Messenger.Common.Elastic
{
    public class ESClient
    {
        public static ElasticClient CreateElasticClient()
        {
            string nodeAddress = DBSettings.ReadSettings("es_address");

            Uri[] nodes = new Uri[]
            {
                new Uri(nodeAddress),
            };

            StaticConnectionPool pool = new StaticConnectionPool(nodes);
            ConnectionSettings settings = new ConnectionSettings(pool);

            return new ElasticClient(settings);
        }

        public static string RequestToReadableString<T>(ElasticClient elasticClient, T request) where T : IRequest
        {
            MemoryStream stream = new MemoryStream();
            elasticClient.RequestResponseSerializer.Serialize(request, stream);
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
    }
}
