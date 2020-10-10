using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.ChatInfoServer.HttpModules
{
    internal static class Routes
    {
        internal const string GET_CHAT_LIST = "/get_chat_list";
        internal const string GET_CHAT_MEMBERS = "/get_chat_members";
        internal const string CREATE_CHAT = "/create_chat";
        internal const string INVITE_TO_CHAT = "/invite_to_chat";
    }
}
