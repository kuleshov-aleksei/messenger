using EmbedIO;
using Messenger.Common.Elastic;
using Messenger.Common.Http;
using Messenger.Common.Settings;
using Messenger.Common.Tools;
using Nest;
using NLog;
using System;
using System.Collections.Generic;

namespace Messenger.MessengerServer
{
    public class Program
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            m_logger.Info("*** Starting ***");

            ElasticClient elasticClient = ESClient.CreateElasticClient();
            IdGenerator idGenerator = new IdGenerator();

            int port = int.Parse(DBSettings.ReadSettings("service_messenger_port"));

            List<IWebModule> webModules = new List<IWebModule>();
            webModules.Add(new HttpModules.PutMessage.PutMessageModule(elasticClient, idGenerator));

            HttpServer httpServer = new HttpServer(port, webModules);
            httpServer.Start();

            Console.ReadKey();
        }
    }
}
