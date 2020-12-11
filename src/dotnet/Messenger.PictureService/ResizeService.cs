using Messenger.RabbitMQ.Common;
using NLog;
using System;

namespace Messenger.PictureService
{
    internal class ResizeService : IDisposable
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private ConsumerWrapper m_consumerWrapper;

        public ResizeService()
        {
            m_logger.Info("Creating resize service");

            m_consumerWrapper = new ConsumerWrapper(QueueName.UpdateUserPicture);
            m_consumerWrapper.OnRawData += OnNewMessage;
        }

        private void OnNewMessage(byte[] data)
        {
            m_logger.Trace("Received new request");
            Request request = Request.Parser.ParseFrom(data);

            if (request.RequestCaseCase != Request.RequestCaseOneofCase.UpdateUserPicture)
            {
                m_logger.Warn($"Unsupported message type: {request.RequestCaseCase}. Should not happen");
                return;
            }

            m_logger.Info($"Received request for updating picture of user {request.UpdateUserPicture.UserId}");

            User user = User.ReadUserFromDb(request.UpdateUserPicture.UserId);

            bool succeed = ImageProcessor.ResizeImages(user);
            if (succeed)
            {
                user.UpdateLinks();
            }
        }

        public void Dispose()
        {
            m_consumerWrapper.Dispose();
        }
    }
}
