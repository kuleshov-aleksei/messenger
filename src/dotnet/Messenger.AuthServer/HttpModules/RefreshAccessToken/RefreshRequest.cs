using Messenger.Common.Http;

namespace Messenger.AuthServer.HttpModules.RefreshAccessToken
{
    public class RefreshRequest : RequestBase
    {
        public override bool Validate()
        {
            return true;
        }
    }
}
