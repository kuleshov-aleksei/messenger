using Messenger.SubscribingService.Models;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.SubscribingService
{
    public class MessageHub
    {
        // One user can have multiple connections: multiple tabs, devices, etc...
        private readonly ConcurrentDictionary<int, ConcurrentBag<WebsocketConnectionHandler>> m_connectedUsers;
        private const int MAX_CONNECTIONS = 10;
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public MessageHub()
        {
            m_connectedUsers = new ConcurrentDictionary<int, ConcurrentBag<WebsocketConnectionHandler>>();
        }

        public async Task Register(WebsocketConnectionHandler websocketConnectionHandler, int userId)
        {
            ConcurrentBag<WebsocketConnectionHandler> userConnections;
            if (m_connectedUsers.ContainsKey(userId))
            {
                userConnections = m_connectedUsers[userId];
                if (userConnections.Count >= MAX_CONNECTIONS - 1)
                {
                    m_logger.Info($"User {userId} already have {userConnections.Count} connections. Disconnecting old one");
                    bool removed = userConnections.TryTake(out WebsocketConnectionHandler oldWebsocketConnectionHandler);
                    if (removed)
                    {
                        await DisconnectUser(oldWebsocketConnectionHandler, ErrorReasons.TooManyDevicesConnected);
                    }
                }
            }
            else
            {
                bool added = m_connectedUsers.TryAdd(userId, new ConcurrentBag<WebsocketConnectionHandler>());
                if (!added)
                {
                    m_logger.Error($"Failed to create new connection pool for user {userId}");
                    return;
                }

                userConnections = m_connectedUsers[userId];
            }

            userConnections.Add(websocketConnectionHandler);
            m_logger.Info($"Added new connection from user {userId} to connection pool. Now this user have {userConnections.Count} active connections");
        }

        private async Task DisconnectUser(WebsocketConnectionHandler websocketConnectionHandler, ErrorReasons errorReason)
        {
            await SendMessage(websocketConnectionHandler, new MessageFromServer
            {
                Code = errorReason,
                ErrorMessage = errorReason.DescibeErrorReason(),
            });

            await websocketConnectionHandler.Close();
        }

        private async Task SendMessage(WebsocketConnectionHandler websocketConnectionHandler, MessageFromServer messageFromServer)
        {
            await websocketConnectionHandler.SendServerMessage(messageFromServer);
        }
    }
}
