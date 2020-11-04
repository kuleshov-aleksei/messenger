using Messenger.Common.Http;

namespace Messenger.UserServer.GetInfo
{
    internal sealed class GetInfoRequest : RequestBase
    {
        public override bool Validate()
        {
            return true;
        }
    }
}
