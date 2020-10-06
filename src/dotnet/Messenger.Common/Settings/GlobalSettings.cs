using MySql.Common;
using System;

namespace Messenger.MessengerServer
{
    public class GlobalSettings
    {
        private static Lazy<GlobalSettings> m_instance = new Lazy<GlobalSettings>(() => new GlobalSettings());
        public static GlobalSettings Instance => m_instance.Value;

        public Database Database { get; }

        private GlobalSettings()
        {
            DatabaseConnectionSettings databaseConnectionSettings = DatabaseConnectionSettings.ReadFromFile("mysql_secrets.json");
            Database = new Database(databaseConnectionSettings);
        }
    }
}
