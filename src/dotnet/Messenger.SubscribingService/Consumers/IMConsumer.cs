using MassTransit;
using MassTransit.Definition;
using Messenger.Common.MassTransit.Models;
using NLog;
using System;
using System.Threading.Tasks;

namespace Messenger.SubscribingService.Consumers
{
    public class IMConsumer : IConsumer<NewMessage>
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private readonly MessageHub m_messageHub;

        public IMConsumer(MessageHub messageHub)
        {
            m_messageHub = messageHub;
        }

        public async Task Consume(ConsumeContext<NewMessage> context)
        {
            m_logger.Trace($"Consuming message from user {context.Message.UserId} to chat {context.Message.ChatId}");
            await m_messageHub.HandleMessage(context.Message.ChatId, context.Message);
        }
    }
}
