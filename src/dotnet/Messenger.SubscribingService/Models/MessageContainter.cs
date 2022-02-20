using Messenger.Common.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Messenger.SubscribingService.Models
{
    public class MessageContainter
    {
        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }
    }
}