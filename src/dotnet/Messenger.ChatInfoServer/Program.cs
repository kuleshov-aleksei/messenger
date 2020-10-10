using EmbedIO;
using Messenger.Common.Http;
using NLog;
using System;
using System.Collections.Generic;

namespace Messenger.ChatInfoServer
{
    class Program
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            m_logger.Info("*** Starting ***");

            List<IWebModule> webModules = new List<IWebModule>();
            webModules.Add(new HttpModules.GetChatList.GetChatListModule());
            webModules.Add(new HttpModules.GetChatMembers.GetChatMembersModule());
            webModules.Add(new HttpModules.CreateChat.CreateChatModule());
            webModules.Add(new HttpModules.InviteToChat.InviteChatModule());

            HttpServer httpServer = new HttpServer(24165, webModules);
            httpServer.Start();

            Console.ReadKey();
        }
    }
}
