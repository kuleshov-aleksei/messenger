using System;
using System.Collections.Generic;

namespace Messenger.RabbitMQ.Common
{
    public enum QueueName : int
    {
        TestQueue = 1,
        UpdateUserPicture,
    }

    public static class Constants
    {
        public static Dictionary<QueueName, string> Queues = new Dictionary<QueueName, string>
        {
            { QueueName.TestQueue, "test_queue" },
            { QueueName.UpdateUserPicture, "update_user_picture" },
        };
    }
}
