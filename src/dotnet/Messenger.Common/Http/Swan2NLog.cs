using Swan.Logging;

namespace Messenger.Common.Http
{
    internal class Swan2NLog : TextLogger, ILogger
    {
        public LogLevel LogLevel => LogLevel.Trace;
        public NLog.Logger m_logger = NLog.LogManager.GetCurrentClassLogger();

        public void Log(LogMessageReceivedEventArgs logEvent)
        {
            m_logger.Log(NLog.LogLevel.Info, $"[{logEvent.CallerMemberName}] {logEvent.Message}");
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
