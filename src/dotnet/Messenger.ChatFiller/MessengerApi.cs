using Fare;
using Messenger.AuthServer.HttpModules.Auth;
using Messenger.ChatInfoServer.HttpModules.GetChatList;
using Messenger.ChatInfoServer.HttpModules.InviteToChat;
using Messenger.InstantMessagingService.Models;
using Messenger.RegistrationService.HttpModules.Register;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.ChatFiller
{
    internal class MessengerApi
    {
        private HttpClient m_httpClient;
        private Xeger m_usernameXeger = new Xeger(@"generated_[a-zA-Z]{10}");

        public MessengerApi()
        {
            m_httpClient = new HttpClient();
        }

        public async Task<(string, string)> Register(string name)
        {
            string userName = m_usernameXeger.Generate();
            string password = "generated_password";

            RegisterRequest registerRequest = new RegisterRequest()
            {
                UserName = userName,
                Email = $"{userName}@encamy.com",
                Name = name,
                Password = password,
                Surname = " "
            };

            string json = JsonConvert.SerializeObject(registerRequest);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            await m_httpClient.PostAsync("https://messenger.local.encamy.com/register/register", httpContent);
            return (userName, password);
        }

        public async Task CreateChat(string title, string token)
        {
            CreateChatRequest createChatRequest = new CreateChatRequest()
            {
                AccessToken = token,
                Title = title,
            };

            string json = JsonConvert.SerializeObject(createChatRequest);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            await m_httpClient.PostAsync("https://messenger.local.encamy.com/chat/create_chat", httpContent);
        }

        public async Task SendMessage(int chatId, string token, string message)
        {
            using (HttpClient client = new HttpClient())
            {
                SendMessageRequest sendMessageRequest = new SendMessageRequest()
                {
                    ChatId = chatId,
                    Message = message,
                };

                string json = JsonConvert.SerializeObject(sendMessageRequest);
                StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage result = await client.PostAsync("https://messenger.local.encamy.com/messenger/instant/send", httpContent);
            }
        }

        public async Task InviteUser(int chatId, string newUser, string token)
        {
            InviteChatRequestByUsername inviteChatRequestByUsername = new InviteChatRequestByUsername()
            {
                AccessToken = token,
                ChatId = chatId,
                InvitedUsername = newUser,
            };

            string json = JsonConvert.SerializeObject(inviteChatRequestByUsername);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            await m_httpClient.PostAsync("https://messenger.local.encamy.com/chat/invite_to_chat_username", httpContent);
        }

        public async Task<int> GetChatId(string title, string token)
        {
            GetChatListRequest getChatListRequest = new GetChatListRequest()
            {
                AccessToken = token,
            };

            string json = JsonConvert.SerializeObject(getChatListRequest);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage result = await m_httpClient.PostAsync("https://messenger.local.encamy.com/chat/get_chat_list", httpContent);
            string response = await result.Content.ReadAsStringAsync();
            ChatList chatList = JsonConvert.DeserializeObject<ChatList>(response);

            return int.Parse(chatList.Chats.FirstOrDefault(x => x.Title == title).Id);
        }

        public async Task<string> Auth(string username, string password)
        {
            AuthRequest authRequest = new AuthRequest()
            {
                Login = username,
                Password = password,
                DeviceName = "ChatFiller",
            };

            string json = JsonConvert.SerializeObject(authRequest);
            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage result = await m_httpClient.PostAsync("https://messenger.local.encamy.com/auth/auth", httpContent);
            string response = await result.Content.ReadAsStringAsync();
            AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(response);
            return authResponse.AccessToken;
        }
    }
}
