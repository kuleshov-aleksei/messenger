using EmbedIO;
using Messenger.Common.Http;
using Messenger.MessengerServer.HttpModules.PutMessage;
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

            List<IWebModule> webModules = new List<IWebModule>();
            //webModules.Add(new HttpModules.GetChatList.GetChatListModule());

            //HttpServer httpServer = new HttpServer(24165, webModules);
            //httpServer.Start();

            Console.ReadKey();
        }
    }
}
