using MySql.Common;
using NLog;
using System;

namespace Messenger.Common.Settings
{
    public class GlobalSettings
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private static Lazy<GlobalSettings> m_instance = new Lazy<GlobalSettings>(() => new GlobalSettings());
        public static GlobalSettings Instance => m_instance.Value;

        public Database Database { get; }
        public const string EsIndexName = "user_messages";
        public const int EsInitialMessagesCount = 30;
        public const int EsMessagesCount = 100;
        public const string EsFieldChatId = "chat_id";
        public const string EsFieldMessageId = "message_id";
        public const string EsFieldText = "text";
        public const string EsFieldUserId = "user_id";
        public const string EsFieldMessageTime = "message_time";

        private GlobalSettings()
        {
            DatabaseConnectionSettings databaseConnectionSettings = DatabaseConnectionSettings.ReadFromFile("mysql_secrets.json");
            m_logger.Info($"Connecting to {databaseConnectionSettings}");
            Database = new Database(databaseConnectionSettings);
        }
    }
}
