using Newtonsoft.Json;
using System;
using System.IO;

namespace MySql.Common
{
    public class DatabaseConnectionSettings
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("database_name")]
        public string DatabaseName { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public static DatabaseConnectionSettings ReadFromFile(string jsonFilename)
        {
            if (!File.Exists(jsonFilename))
            {
                throw new ApplicationException("File does not exists");
            }

            return JsonConvert.DeserializeObject<DatabaseConnectionSettings>(File.ReadAllText(jsonFilename));
        }
    }
}
