using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Data;

namespace MySql.Common
{
    public class DatabaseConnection : IDisposable
    {
        private MySqlConnection m_mysqlConnection;
        private Logger m_logger = LogManager.GetCurrentClassLogger();

        public DatabaseConnection(string connectionString)
        {
            m_mysqlConnection = new MySqlConnection(connectionString);
            m_logger.Trace($"Creating mysql wrapper for server {m_mysqlConnection.Site}");

            m_mysqlConnection.StateChange += OnMysqlConnectionStateChange;
            m_mysqlConnection.Open();
        }

        private void OnMysqlConnectionStateChange(object sender, StateChangeEventArgs e)
        {
            m_logger.Trace($"State changed: prev state = {e.OriginalState} new state = {e.CurrentState}");
            if (e.CurrentState == ConnectionState.Closed)
            {
                Open();
            }
        }

        public void Open()
        {
            ConnectionState connectionState = m_mysqlConnection.State;
            m_mysqlConnection.Open();
            m_logger.Trace($"Openning connection. Previous state {connectionState}, current state {m_mysqlConnection.State}");
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
