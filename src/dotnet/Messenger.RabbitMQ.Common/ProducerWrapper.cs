using Messenger.Common.Settings;
using NLog;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Messenger.RabbitMQ.Common
{
    public class ProducerWrapper : IDisposable
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private IConnection m_mqConnection;
        private IModel m_mqChannel;
        private string m_queueName;

        public ProducerWrapper(QueueName queueName)
        {
            string address = DBSettings.ReadSettings("rabbit_mq_address");
            string username = DBSettings.ReadSettings("rabbit_mq_user");
            string password = DBSettings.ReadSettings("rabbit_mq_password");
            m_logger.Info($"Creating producer for Rabbit MQ at \"{address}\"");

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Address of rabbit mq can't be empty");
            }

            ConnectionFactory factory = new ConnectionFactory() { 
                HostName = address,
                UserName = username,
                Password = password,
            };

            m_mqConnection = factory.CreateConnection();
            m_mqChannel = m_mqConnection.CreateModel();
            m_queueName = Constants.Queues[queueName];

            QueueDeclareOk queueDeclareOk = m_mqChannel.QueueDeclare(
                queue: m_queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            m_logger.Info($"Queue \"{queueDeclareOk.QueueName}\" declared. Have {queueDeclareOk.MessageCount} messages and {queueDeclareOk.ConsumerCount} consumers");
        }

        public void Produce(string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);
            Produce(body);
        }

        public void Produce(byte[] data)
        {
            m_logger.Trace("Sending message to MQ");
            m_mqChannel.BasicPublish(exchange: "",
                                     routingKey: m_queueName,
                                     basicProperties: null,
                                     body: data);
        }

        public void Dispose()
        {
            m_mqChannel.Dispose();
            m_mqConnection.Dispose();
        }
    }
}
