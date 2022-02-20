using Messenger.SubscribingService.Models;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.SubscribingService
{
    public class MessageHub
    {
        // One user can have multiple connections: multiple tabs, devices, etc...
        private readonly ConcurrentDictionary<int, ConcurrentDictionary<Guid, WebsocketConnectionHandler>> m_connectedUsers;
        private const int MAX_CONNECTIONS = 10;
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public MessageHub()
        {
            m_connectedUsers = new ConcurrentDictionary<int, ConcurrentDictionary<Guid, WebsocketConnectionHandler>>();
        }

        public async Task Register(WebsocketConnectionHandler websocketConnectionHandler, int userId, Guid guid)
        {
            websocketConnectionHandler.OnClose += HandleConnectionCloseFromClient;

            ConcurrentDictionary<Guid, WebsocketConnectionHandler> userConnections;
            if (m_connectedUsers.ContainsKey(userId))
            {
                userConnections = m_connectedUsers[userId];
                if (userConnections.Count >= MAX_CONNECTIONS - 1)
                {
                    m_logger.Info($"User {userId} already have {userConnections.Count} connections. Disconnecting old one");
                    bool removed = userConnections.TryRemove(userConnections.First().Key, out WebsocketConnectionHandler oldWebsocketConnectionHandler);
                    if (removed)
                    {
                        m_logger.Info($"Disconnecting user {oldWebsocketConnectionHandler.GetGuid()}");
                        await DisconnectUser(oldWebsocketConnectionHandler, ErrorReasons.TooManyDevicesConnected);
                    }
                }
            }
            else
            {
                bool multiplexerAdded = m_connectedUsers.TryAdd(userId, new ConcurrentDictionary<Guid, WebsocketConnectionHandler>());
                if (!multiplexerAdded)
                {
                    m_logger.Error($"Failed to create new connection pool for user {userId}");
                    return;
                }

                userConnections = m_connectedUsers[userId];
            }

            bool added = userConnections.TryAdd(guid, websocketConnectionHandler);
            if (added)
            {
                m_logger.Info($"Added new connection from user {userId} to connection pool. Now this user have {userConnections.Count} active connections");
            }
            else
            {
                m_logger.Error($"Tried to add new connection, but failed");
            }
        }

        private void HandleConnectionCloseFromClient(int userId, Guid guid, WebsocketConnectionHandler websocketConnectionHandler)
        {
            if (!m_connectedUsers.ContainsKey(userId) || !m_connectedUsers[userId].ContainsKey(guid))
            {
                return;
            }

            bool removed = m_connectedUsers[userId].TryRemove(guid, out _);
            if (!removed)
            {
                m_logger.Error($"Failed to remove connection {guid} for disconnect event");
            }
            else
            {
                m_logger.Info($"Removed connection from user {userId} from connection pool. Now this user have {m_connectedUsers[userId].Count} active connections");
            }
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
