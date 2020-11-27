using Messenger.RabbitMQ.Common;
using NLog;
using System.Threading;

namespace Messenger.RabbitMQ.Example
{
    class Program
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            m_logger.Info("Starting");

            using ProducerWrapper producerWrapper = new ProducerWrapper(QueueName.TestQueue);
            using ConsumerWrapper consumerWrapper = new ConsumerWrapper(QueueName.TestQueue);

            consumerWrapper.OnString += (message) =>
            {
                m_logger.Info($"Got message: \"{message}\"");
            };

            consumerWrapper.Start();

            producerWrapper.Produce("Hello world!");
            producerWrapper.Produce("Test message");

            Thread.Sleep(500);
            producerWrapper.Produce("Message after sleep");

            Thread.Sleep(500);
        }
    }
}
