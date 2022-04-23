using Newtonsoft.Json;

namespace Messenger.Fileserver.Models
{
    public class GetSharableUrlModel
    {
        [JsonProperty("object_name")]
        public string ObjectName { get; set; }
    }
}
