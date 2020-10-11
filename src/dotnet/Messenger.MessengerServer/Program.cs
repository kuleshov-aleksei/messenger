using EmbedIO;
using Messenger.Common.Elastic;
using Messenger.Common.Http;
using Messenger.Common.Settings;
using Messenger.Common.Tools;
using Nest;
using NLog;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;

namespace Messenger.MessengerServer
{
    public class Program
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();
        private static volatile bool m_running;

        static void MainWrapped(string[] args)
        {
            m_logger.Info("*** Starting ***");

            ElasticClient elasticClient = ESClient.CreateElasticClient();
            EsInteractor esInteractor = new EsInteractor(elasticClient);
            IdGenerator idGenerator = new IdGenerator();

            int port = int.Parse(DBSettings.ReadSettings("service_messenger_port"));

            List<IWebModule> webModules = new List<IWebModule>();
            webModules.Add(new HttpModules.PutMessage.PutMessageModule(elasticClient, idGenerator));
            webModules.Add(new HttpModules.GetLastMessages.GetLastMessagesModule(esInteractor));
            webModules.Add(new HttpModules.GetMessagesFrom.GetMessagesFromModule(esInteractor));

            HttpServer httpServer = new HttpServer(port, webModules);
            httpServer.Start();

            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);

            AssemblyLoadContext.Default.Unloading += SigTermHandler;

            try
            {
                MainWrapped(args);
            }
            catch (Exception e)
            {
                m_logger.Fatal(e);
            }
        }

        private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;
            m_logger.Fatal(exception);
        }

        private static void SigTermHandler(AssemblyLoadContext obj)
        {
            m_running = false;
        }
    }
}
