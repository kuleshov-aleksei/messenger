using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Common.Elastic.Models
{
    public class EsMessage
    {
        public int ChatId { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public long MessageTime { get; set; }
        public string ImageUrlOriginal { get; set; }
        public string AttachmentUrl { get; set; }
    }
}
