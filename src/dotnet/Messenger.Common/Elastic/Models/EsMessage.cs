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
        public int MessageTime { get; set; }
    }
}
