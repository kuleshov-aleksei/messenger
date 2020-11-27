using Messenger.Common.Settings;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Messenger.RabbitMQ.Common
{
    public class ConsumerWrapper : IDisposable
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private IConnection m_mqConnection;
        private IModel m_mqChannel;
        private EventingBasicConsumer m_mqConsumer;
        private string m_queueName;

        public Action<byte[]> OnRawData;
        public Action<string> OnString;

        public ConsumerWrapper(QueueName queueName)
        {
            string address = DBSettings.ReadSettings("rabbit_mq_address");
            string username = DBSettings.ReadSettings("rabbit_mq_user");
            string password = DBSettings.ReadSettings("rabbit_mq_password");
            m_logger.Info($"Creating producer for Rabbit MQ at \"{address}\"");

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Address of rabbit mq can't be empty");
            }

            ConnectionFactory factory = new ConnectionFactory()
            {
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

            m_mqConsumer = new EventingBasicConsumer(m_mqChannel);
            m_mqConsumer.Received += OnConsumerReceived;
        }

        public void Start()
        {
            m_mqChannel.BasicConsume(
                queue: m_queueName,
                autoAck: true,
                consumer: m_mqConsumer);
        }

        private void OnConsumerReceived(object model, BasicDeliverEventArgs ea)
        {
            byte[] data = ea.Body.ToArray();

            if (OnRawData != null)
            {
                OnRawData.Invoke(data);
            }
            else
            {
                string message = Encoding.UTF8.GetString(data);
                OnString?.Invoke(message);
            }
        }

        public void Dispose()
        {
            m_mqChannel.Dispose();
            m_mqConnection.Dispose();
        }
    }
}
