using Messenger.Common.Http.Status;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Net;

namespace Messenger.Orchestrator
{
    internal class Service
    {
        [JsonIgnore]
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public StatusResponse LastStatus { get; set; }

        public override string ToString()
        {
            return $"{Name, -20} ({Description, -25}) on {Address}:{Port}";
        }

        public StatusResponse GetStatus()
        {
            try
            {
                string url = $"http://{Address}:{Port}/status";
                m_logger.Info($"Sending GET request to {url}");

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.Timeout = 10_000;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string responseString = reader.ReadToEnd();
                    StatusResponse status = JsonConvert.DeserializeObject<StatusResponse>(responseString);
                    LastStatus = status;
                    LastStatus.CreationDate = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");
                    m_logger.Info($"Got response from {Name}");
                }
            }
            catch (Exception e)
            {
                m_logger.Error($"Failed to read status of service {Name}. {e.Message}, {e.StackTrace}");
                LastStatus = null;
            }

            return LastStatus;
        }
    }
}
