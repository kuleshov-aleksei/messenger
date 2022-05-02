using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Common.Elastic.Models
{
    public class ElasticDocument
    {
        public long message_time { get; set; }
        public int user_id { get; set; }
        public string text { get; set; }
        public int chat_id { get; set; }
        public string image_url_small { get; set; }
        public string image_url_original { get; set; }
        public string image_url_large { get; set; }
        public string image_url_medium { get; set; }
        public string attachment_url { get; set; }
    }
}
