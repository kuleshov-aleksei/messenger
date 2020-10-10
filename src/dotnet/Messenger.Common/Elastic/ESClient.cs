using Elasticsearch.Net;
using Messenger.Common.Settings;
using Nest;
using System;

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
    }
}
