using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.MessengerServer
{
    public class ElasticDocument
    {
        public long message_time { get; set; }
        public int user_id { get; set; }
        public string text { get; set; }
        public int chat_id { get; set; }
    }
}
