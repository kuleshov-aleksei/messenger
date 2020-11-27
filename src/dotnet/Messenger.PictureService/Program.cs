using NLog;
using System;
using System.Runtime.Loader;
using System.Threading;

namespace Messenger.PictureService
{
    class Program
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();
        private static volatile bool m_running;

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

        static void MainWrapped(string[] args)
        {
            m_logger.Info("*** Starting ***");

            m_running = true;
            while (m_running)
            {
                Thread.Sleep(100);
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
