using Messenger.Common.Http;
using Newtonsoft.Json;

namespace Messenger.ChatInfoServer.HttpModules.GetChatList
{
    public class GetChatListRequest : RequestBase
    {
        public override bool Validate()
        {
            return true;
        }
    }
}
