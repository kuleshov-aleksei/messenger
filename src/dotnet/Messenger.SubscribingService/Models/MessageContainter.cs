using Messenger.Common.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Messenger.SubscribingService.Models
{
    public class MessageContainter
    {
        [JsonProperty("message")]
        public Message Message { get; set; }
    }
}