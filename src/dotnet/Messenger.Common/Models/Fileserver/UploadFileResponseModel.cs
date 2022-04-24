using Newtonsoft.Json;
using System.Collections.Generic;

namespace Messenger.Common.Models.Fileserver
{
    public class UploadFileResponseModel
    {
        [JsonProperty("files")]
        public List<string> Files { get; set; }
    }
}
