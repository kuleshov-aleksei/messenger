using Grpc.Core;
using Messenger.Common.JWT;
using Nest;
using NLog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.gRPC
{
    internal class MessengerServiceImpl : MessengerService.MessengerServiceBase
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private EsInteractor m_esInteractor;
        private JwtHelper m_jwtHelper;

        public MessengerServiceImpl(EsInteractor esInteractor, JwtHelper jwtHelper)
        {
            m_esInteractor = esInteractor;
            m_jwtHelper = jwtHelper;
        }

        public override Task<EchoResponse> Echo(EchoRequest request, ServerCallContext context)
        {
            m_logger.Info("Got request echo: " + request.Message);

            return Task.FromResult(new EchoResponse
            {
                Message = "echo " + request.Message
            });
        }

        public override Task<ServerResponse> GetLastMessages(GetLastMessagesRequest request, ServerCallContext context)
        {
            m_logger.Info("Got request for loading last messages");

            if (request == null || string.IsNullOrEmpty(request.AccessToken))
            {
                m_logger.Info("Empty access token");
                return CreateErrorResponse(HttpStatusCode.Unauthorized);
            }

            if (request.ChatId <= 0)
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest);
            }

            bool authorized = m_jwtHelper.Validate(request.AccessToken, out int userId);
            if (!authorized)
            {
                m_logger.Info("Invalid access token");
                return CreateErrorResponse(HttpStatusCode.Unauthorized);
            }

            m_logger.Info($"Returning messages of chat {request.ChatId}");

            MessagesResponse messagesResponse = m_esInteractor.GetLastMessagesOfChat(request.ChatId);
            ServerResponse serverResponse = new ServerResponse
            {
                MessageList = new MessageList()
            };

            serverResponse.MessageList.ChatId = messagesResponse.ChatId;

            foreach (Message message in messagesResponse.Messages)
            {
                serverResponse.MessageList.Messages.Add(new global::Message
                {
                    AuthorImage = message.AuthorImageLinkSmall ?? string.Empty,
                    AuthorName = message.AuthorName,
                    AuthorSurname = message.AuthorSurname,
                    Text = message.Text,
                    UnixTime = message.UnixTime
                });
            }

            return Task.FromResult(serverResponse);
        }

        public override Task<ServerResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            return base.SendMessage(request, context);
        }

        private Task<ServerResponse> CreateErrorResponse(HttpStatusCode statusCode)
        {
            return Task.FromResult(new ServerResponse
            {
                ErrorMessage = new Error
                {
                    ErrorMessage = $"{(int)statusCode} {statusCode}"
                }
            });
        }
    }
}
