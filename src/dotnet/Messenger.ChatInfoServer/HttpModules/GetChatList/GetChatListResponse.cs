using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Messenger.ChatInfoServer.HttpModules.GetChatList
{
    public class ChatList
    {
        [JsonProperty("chats")]
        public List<Chat> Chats { get; set; } = new List<Chat>();
    }

    public class Chat
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image_medium")]
        public string ImageMedium { get; set; }

        [JsonProperty("image_small")]
        public string ImageSmall { get; set; }

        [JsonProperty("last_message")]
        public LastMessage LastMessage { get; set; }
    }

    public class LastMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_surname")]
        public string AuthorSurname { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
