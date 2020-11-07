using EmbedIO;
using Messenger.Common.Http;
using Messenger.Common.JWT;
using Messenger.Common.Settings;
using Messenger.UserServer.GetInfo;
using NLog;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Threading;

namespace Messenger.UserServer
{
    class Program
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();
        private static volatile bool m_running;

        static void MainWrapped(string[] args)
        {
            m_logger.Info("*** Starting ***");

            int port = int.Parse(DBSettings.ReadSettings("service_userinfo_port"));

            JwtHelper jwtHelper = new JwtHelper("jwt_secret.secret");

            List<IWebModule> webModules = new List<IWebModule>();
            webModules.Add(new GetInfoModule(jwtHelper));
            webModules.Add(new ChangeName.ChangeNameModule(jwtHelper));
            webModules.Add(new ChangeSurname.ChangeSurnameModule(jwtHelper));
            webModules.Add(new ChangeEmail.ChangeEmailModule(jwtHelper));
            webModules.Add(new ChangePassword.ChangePasswordModule(jwtHelper));

            HttpServer httpServer = new HttpServer(port, webModules);
            httpServer.Start();

            m_running = true;
            while (m_running)
            {
                Thread.Sleep(100);
            }
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
