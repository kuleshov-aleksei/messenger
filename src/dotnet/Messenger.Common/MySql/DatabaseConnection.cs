using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Data;

namespace Messenger.Common.MySql
{
    public class DatabaseConnection : IDisposable
    {
        private MySqlConnection m_mysqlConnection;
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public DatabaseConnection(string connectionString)
        {
            m_mysqlConnection = new MySqlConnection(connectionString);
            m_mysqlConnection.Open();
        }

        public MySqlCommand CreateCommand()
        {
            return m_mysqlConnection.CreateCommand();
        }

        public void Dispose()
        {
            m_mysqlConnection.Dispose();
        }
    }
}
