using EmbedIO;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Common.Http
{
    public class HttpServer : IDisposable
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private WebServer m_webServer;

        public HttpServer(int listenPort)
        {
            Swan.Logging.Logger.NoLogging();
            Swan.Logging.Logger.RegisterLogger(new Swan2NLog());

            m_webServer = new WebServer(o => o
                .WithUrlPrefix($"http://*:{listenPort}/")
                .WithMode(HttpListenerMode.EmbedIO))
                .WithCors()
                .WithModule(new PingModule())
                .WithLocalSessionManager()
                .HandleUnhandledException(HandleException)
                .HandleHttpException(HttpExceptionHandlerFunc);

            m_webServer.StateChanged += (s, e) => m_logger.Trace($"New WebServer state {e.NewState}");
        }

        public void Start()
        {
            m_webServer.Start();
        }

        private async Task HttpExceptionHandlerFunc(IHttpContext context, IHttpException httpException)
        {
            context.Response.StatusCode = httpException.StatusCode;

            m_logger.Error($"Got http exception with status code {httpException.StatusCode} {httpException.Message}");
            await context.SendStandardHtmlAsync(httpException.StatusCode);
        }

        private async Task HandleException(IHttpContext context, Exception exception)
        {
            m_logger.Error($"Got error while processing request: " + exception.Message + Environment.NewLine + exception.StackTrace);
            await ModuleBase.SendResponse(context, System.Net.HttpStatusCode.InternalServerError);
        }

        public void Dispose()
        {
            m_webServer.Dispose();
        }
    }
}
