using System;
using System.Collections.Generic;

namespace Messenger.RabbitMQ.Common
{
    public enum QueueName : int
    {
        NewUser = 1,
    }

    public static class Constants
    {
        public static Dictionary<QueueName, string> Queues = new Dictionary<QueueName, string>
        {
            { QueueName.NewUser, "new_user" },
        };
    }
}
