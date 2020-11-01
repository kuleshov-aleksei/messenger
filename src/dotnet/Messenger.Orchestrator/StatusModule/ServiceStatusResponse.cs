using Messenger.Common.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Messenger.Orchestrator.StatusModule
{
    internal class ServiceStatusResponse : ResponseBase
    {
        [JsonProperty("services")]
        public List<Service> Services { get; set; }
    }
}
