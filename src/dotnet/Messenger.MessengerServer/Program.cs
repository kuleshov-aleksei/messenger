using EmbedIO;
using Grpc.Core;
using Messenger.Common.Elastic;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using Messenger.Common.Settings;
using Messenger.Common.Tools;
using Messenger.MessengerServer.gRPC;
using Nest;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Threading;

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
            IdGenerator idGenerator = new IdGenerator();
            EsInteractor esInteractor = new EsInteractor(elasticClient, idGenerator);

            int port = int.Parse(DBSettings.ReadSettings("service_messenger_port"));

            JwtHelper jwtHelper = new JwtHelper("jwt_secret.secret");

            List<IWebModule> webModules = new List<IWebModule>();
            webModules.Add(new HttpModules.PutMessage.PutMessageModule(esInteractor, jwtHelper));
            webModules.Add(new HttpModules.GetLastMessages.GetLastMessagesModule(esInteractor, jwtHelper));
            webModules.Add(new HttpModules.GetMessagesFrom.GetMessagesFromModule(esInteractor, jwtHelper));

            HttpServer httpServer = new HttpServer(port, webModules);
            httpServer.Start();

            Server grpcServer = new Server
            {
                Services = { MessengerService.BindService(new MessengerServiceImpl(esInteractor, jwtHelper)) },
                Ports = { { "0.0.0.0", 7813, ServerCredentials.Insecure } }
            };
            grpcServer.Start();

            m_logger.Info($"gRPC server is listening on {grpcServer.Ports.ElementAt(0).Host}:{grpcServer.Ports.ElementAt(0).Port}");

            m_running = true;
            while (m_running)
            {
                Thread.Sleep(100);
            }

            grpcServer.ShutdownAsync().Wait();
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
