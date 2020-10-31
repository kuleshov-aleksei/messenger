using Grpc.Core;
using Messenger.Common;
using Messenger.Common.JWT;
using Messenger.Common.Tools;
using MySql.Common;
using NLog;
using Swan;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
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

            if (request == null)
            {
                m_logger.Info("Empty request");
                return CreateErrorResponse(HttpStatusCode.BadRequest);
            }

            if (request.ChatId <= 0)
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest);
            }

            Task<ServerResponse> isValid = ValidateJWT(request.AccessToken, out int userId);
            if (isValid != null)
            {
                return isValid;
            }

            m_logger.Info($"Returning messages of chat {request.ChatId}");

            MessagesResponse messagesResponse = m_esInteractor.GetLastMessagesOfChat(request.ChatId);
            ServerResponse serverResponse = new ServerResponse
            {
                MessageList = new MessageList()
            };

            if (messagesResponse == null)
            {
                return Task.FromResult(new ServerResponse
                {
                    Empty = new Empty()
                });
            }

            serverResponse.MessageList.ChatId = messagesResponse.ChatId;

            foreach (Message message in messagesResponse.Messages)
            {
                serverResponse.MessageList.Messages.Add(new global::Message
                {
                    AuthorImage = message.AuthorImageLinkSmall ?? string.Empty,
                    AuthorName = message.AuthorName,
                    AuthorSurname = message.AuthorSurname,
                    Text = message.Text,
                    UnixTime = message.UnixTime,
                    IsAuthor = message.AuthorId == userId
                });
            }

            return Task.FromResult(serverResponse);
        }

        public override Task<ServerResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            m_logger.Info("Got request for sending message");

            if (request == null)
            {
                m_logger.Info("Empty request");
                return CreateErrorResponse(HttpStatusCode.BadRequest);
            }

            if (request.ChatId <= 0)
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(request.Message))
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest);
            }

            Task<ServerResponse> isValid = ValidateJWT(request.AccessToken, out int userId);
            if (isValid != null)
            {
                return isValid;
            }

            m_logger.Info($"Writing message from user {userId} to chat {request.ChatId}");

            bool unathorized = false;
            GlobalSettings.Instance.Database.ExecuteSql(
                $@"SELECT COUNT(*) AS 'count'
                FROM `v_chat_members`
                WHERE `chat_id` = {request.ChatId}
                AND `user_id` = {userId}",
                reader =>
                {
                    int? count = reader.GetInt32("count");
                    if (!count.HasValue)
                    {
                        unathorized = true;
                        return;
                    }

                    if (count.Value == 0)
                    {
                        unathorized = true;
                        return;
                    }
                });

            if (unathorized)
            {
                return CreateErrorResponse(HttpStatusCode.Unauthorized);
            }

            EsMessage message = new EsMessage
            {
                ChatId = request.ChatId,
                Message = request.Message,
                UserId = userId,
            };

            bool putResult = m_esInteractor.PutMessage(message);

            if (!putResult)
            {
                return Task.FromResult(new ServerResponse
                {
                    ErrorMessage = new Error
                    {
                        ErrorMessage = "Не удалось отправить сообщение"
                    }
                });
            }

            m_logger.Info($"Message written");

            return Task.FromResult(new ServerResponse
            {
                Empty = new Empty()
            });
        }

        public override async Task SubscribeToMessages(SubscribeRequest request, IServerStreamWriter<ServerResponse> responseStream, ServerCallContext context)
        {
            m_logger.Info("Got subscribe request");

            Task<ServerResponse> isValid = ValidateJWT(request.AccessToken, out int userId);
            if (isValid != null)
            {
                await responseStream.WriteAsync(isValid.Result);
                return;
            }

            if (request.ChatId <= 0)
            {
                await responseStream.WriteAsync(CreateErrorResponse(HttpStatusCode.BadRequest).Result);
                return;
            }

            m_logger.Info($"Subscribing user {userId} for updates of chat {request.ChatId}");

            long lastMessageTime = UnixEpochTools.ToEpoch(DateTime.UtcNow);
            Stopwatch elapsed = new Stopwatch();
            elapsed.Start();
            while (!context.CancellationToken.IsCancellationRequested)
            {
                ServerResponse response = GetMessagesFrom(lastMessageTime, request.ChatId, userId);

                if (response != null)
                {
                    m_logger.Debug($"Got {response.MessageList.Messages.Count} new messages, sending");
                    await responseStream.WriteAsync(response);
                    lastMessageTime = response.MessageList.Messages.Max(x => x.UnixTime);
                }
                else
                {
                    if (elapsed.ElapsedMilliseconds > 5000)
                    {
                        m_logger.Debug($"Sending empty");
                        await responseStream.WriteAsync(new ServerResponse
                        {
                            Empty = new Empty()
                        });
                        elapsed.Restart();
                    }

                    await Task.Delay(100);
                }
            }
            elapsed.Stop();

            m_logger.Info($"Subsctiption for user {userId} expired");
        }

        private ServerResponse GetMessagesFrom(long unixTime, int chatId, int userId)
        {
            MessagesResponse messagesResponse = m_esInteractor.GetMessagesFrom(unixTime, chatId);

            if (messagesResponse == null || messagesResponse.Messages.Count == 0)
            {
                return null;
            }

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
                    UnixTime = message.UnixTime,
                    IsAuthor = message.AuthorId == userId
                });
            }

            return serverResponse;
        }

        private Task<ServerResponse> ValidateJWT(string token, out int userId)
        {
            if (string.IsNullOrEmpty(token))
            {
                m_logger.Info("Empty access token");
                userId = 0;
                return CreateErrorResponse(HttpStatusCode.Unauthorized);
            }

            bool authorized = m_jwtHelper.Validate(token, out userId);
            if (!authorized)
            {
                m_logger.Info("Invalid access token");
                return CreateErrorResponse(HttpStatusCode.Unauthorized);
            }

            return null;
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
