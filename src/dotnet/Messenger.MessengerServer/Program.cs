using Messenger.Common.Http;
using NLog;
using System;

namespace Messenger.MessengerServer
{
    class Program
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            m_logger.Info("Starting");

            HttpServer httpServer = new HttpServer(24165);
            httpServer.Start();

            Console.ReadKey();
        }
    }
}
