using MySql.Data.MySqlClient;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Messenger.Common.MySql
{
    public class Database
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private string m_connectionString;

        public Database(DatabaseConnectionSettings databaseConnectionSettings)
        {
            string connectionString =
                $"server={databaseConnectionSettings.Host};" +
                $"port={databaseConnectionSettings.Port};" +
                $"database={databaseConnectionSettings.DatabaseName};" +
                $"user={databaseConnectionSettings.User};" +
                $"password={databaseConnectionSettings.Password}";

            m_connectionString = connectionString;
        }

        public async Task ExecuteSqlAsync(string sql, Action<IDataReader> onRow = null, Dictionary<string, object> parameters = null)
        {
            using (DatabaseConnection dbConnection = new DatabaseConnection(m_connectionString))
            {
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = sql;

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> keyValuePair in parameters)
                    {
                        command.Parameters.AddWithValue($"@{keyValuePair.Key}", keyValuePair.Value);
                    }
                }

                if (onRow == null)
                {
                    await command.ExecuteNonQueryAsync();
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
        }

        public void ExecuteSql(string sql, Action<IDataReader> onRow = null, Dictionary<string, object> parameters = null)
        {
            ExecuteSqlAsync(sql, onRow, parameters).Wait();
        }

        public void ExecuteProcedure(string procedureName, List<Tuple<string, object, ParameterDirection, MySqlDbType>> parameters, out Dictionary<string, object> returnValues)
        {
            m_logger.Trace($"Executing procedure {procedureName}");

            using (DatabaseConnection dbConnection = new DatabaseConnection(m_connectionString))
            {
                MySqlCommand command = dbConnection.CreateCommand();
                command.CommandText = procedureName;
                command.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < parameters.Count; i++)
                {
                    Tuple<string, object, ParameterDirection, MySqlDbType> parameter = parameters[i];

                    string parameterName = $"@{parameter.Item1}";
                    if (parameter.Item3 == ParameterDirection.Input)
                    {
                        command.Parameters.AddWithValue(parameterName, parameter.Item2);
                        command.Parameters[parameterName].Direction = ParameterDirection.Input;
                    }
                    else if (parameter.Item3 == ParameterDirection.Output)
                    {
                        command.Parameters.Add(parameterName, parameter.Item4);
                        command.Parameters[parameterName].Direction = ParameterDirection.Output;
                    }
                }

                command.ExecuteNonQuery();

                returnValues = new Dictionary<string, object>();

                for (int i = 0; i < parameters.Count; i++)
                {
                    Tuple<string, object, ParameterDirection, MySqlDbType> parameter = parameters[i];

                    string parameterName = $"@{parameter.Item1}";
                    if (parameter.Item3 == ParameterDirection.Output)
                    {
                        returnValues.Add(parameter.Item1, command.Parameters[parameterName].Value);
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
            using (DatabaseConnection dbConnection = new DatabaseConnection(m_connectionString))
            {
                MySqlCommand command = dbConnection.CreateCommand();
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
}
