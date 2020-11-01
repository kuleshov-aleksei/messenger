using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Common.Http.Status
{
    public class StatusResponse
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("machine_name")]
        public string MachineName { get; set; }

        [JsonProperty("os_version")]
        public string OSVersion { get; set; }

        [JsonProperty("processor_count")]
        public int ProcessorCount { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("used_memory")]
        public string UsedMemory { get; set; }

        [JsonProperty("total_memory")]
        public string TotalMemory { get; set; }

        [JsonProperty("free_memory")]
        public string FreeMemory { get; set; }

        [JsonProperty("used_memory_percentage")]
        public int UsedMemoryPercent { get; set; }

        [JsonProperty("start_time")]
        public string StartTime { get; set; }

        [JsonProperty("working_time")]
        public string WorkingTime { get; set; }

        [JsonProperty("creation_date")]
        public string CreationDate { get; set; }
    }
}
