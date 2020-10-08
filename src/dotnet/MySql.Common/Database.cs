using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MySql.Common
{
    public class Database
    {
        private MySqlConnection m_mySqlConnection;

        public Database(DatabaseConnectionSettings databaseConnectionSettings)
        {
            string connectionString = 
                $"server={databaseConnectionSettings.Host};" +
                $"port={databaseConnectionSettings.Port};" +
                $"database={databaseConnectionSettings.DatabaseName};" +
                $"user={databaseConnectionSettings.User};" +
                $"password={databaseConnectionSettings.Password}";

            m_mySqlConnection = new MySqlConnection(connectionString);

            try
            {
                m_mySqlConnection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ExecuteSql(string sql, Action<IDataReader> onRow = null)
        {
            MySqlCommand command = m_mySqlConnection.CreateCommand();
            command.CommandText = sql;

            if (onRow == null)
            {
                command.ExecuteNonQuery();
            }
            else
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        onRow(reader);
                    }
                }
            }
        }

        public void ExecuteProcedure(string procedureName, Dictionary<string, object> parameters = null)
        {
            ExecuteProcedureAsync(procedureName, parameters).Wait();
        }

        public async Task ExecuteProcedureAsync(string procedureName, Dictionary<string, object> parameters = null)
        {
            MySqlCommand command = m_mySqlConnection.CreateCommand();
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, object> keyValuePair in parameters)
            {
                command.Parameters.AddWithValue($"@{keyValuePair.Key}", keyValuePair.Value);
            }

            await command.ExecuteNonQueryAsync();
        }
    }
}
