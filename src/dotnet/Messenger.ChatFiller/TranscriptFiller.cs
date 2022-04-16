using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.ChatFiller
{
    internal class TranscriptFiller
    {
        private string m_path;
        private readonly MessengerApi m_messengerApi;
        private readonly string m_token;
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public TranscriptFiller(string path, MessengerApi messengerApi, string token)
        {
            m_path = path;
            m_messengerApi = messengerApi;
            m_token = token;
        }

        internal async Task Process()
        {
            IEnumerable<string> scripts = Directory.EnumerateFiles(m_path, "*.txt");
            foreach (string script in scripts)
            {
                TranscriptReader reader = new TranscriptReader(script);
                reader.Read();

                string name = reader.GetName();
                List<string> rawActors = reader.GetActors();
                List<TranscriptReader.Line> lines = reader.GetLines();

                List<Actor> actors = new List<Actor>();

                m_logger.Trace($"Creating chat {name}");
                await m_messengerApi.CreateChat(name, m_token);
                int chatId = await m_messengerApi.GetChatId(name, m_token);

                m_logger.Info($"========= {name} =========");
                m_logger.Info("Actors: ");
                foreach (string actor in rawActors)
                {
                    m_logger.Trace($"Registering user {actor}");
                    (string username, string password) = await m_messengerApi.Register(actor);
                    string token = await m_messengerApi.Auth(username, password);
                    actors.Add(new Actor(actor, token));

                    m_logger.Trace($"Inviting {actor} ({username}) to chat {chatId}");
                    await m_messengerApi.InviteUser(chatId, username, m_token);
                }

                m_logger.Info("Lines: ");
                foreach (TranscriptReader.Line line in lines)
                {
                    Actor actor = actors.FirstOrDefault(x => x.Name == line.Actor);
                    m_logger.Trace($"Sending message from {actor}: {line.Replica}");
                    await m_messengerApi.SendMessage(chatId, actor.Token, line.Replica);
                    await Task.Delay(50);
                }
            }
        }

        public record Actor (string Name, string Token);
    }
}