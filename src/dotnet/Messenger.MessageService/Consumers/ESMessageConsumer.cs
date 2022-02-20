using MassTransit;
using MassTransit.Definition;
using Messenger.Common.Elastic;
using Messenger.Common.MassTransit.Models;
using NLog;
using System.Threading.Tasks;

namespace Messenger.MessageService.Consumers
{
    public class ESMessageConsumer : IConsumer<NewMessage>
    {
        private readonly EsInteractor m_esInteractor;
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public ESMessageConsumer(EsInteractor esInteractor)
        {
            m_esInteractor = esInteractor;
        }

        public async Task Consume(ConsumeContext<NewMessage> context)
        {
            m_logger.Trace($"Consuming message from user {context.Message.UserId}");

            await m_esInteractor.PutMessageAsync(new Common.Elastic.Models.EsMessage
            {
                Message = context.Message.Message,
                ChatId = context.Message.ChatId,
                UserId = context.Message.UserId,
                MessageTime = context.Message.MessageTime,
            });
        }
    }

    public class ESMessageConsumerDefinition
        : ConsumerDefinition<ESMessageConsumer>
    {
        public ESMessageConsumerDefinition()
        {
            EndpointName = "incoming-messages";
        }
    }
}
