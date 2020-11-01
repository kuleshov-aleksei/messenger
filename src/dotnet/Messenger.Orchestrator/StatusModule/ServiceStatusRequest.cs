using Messenger.Common.Http;

namespace Messenger.Orchestrator.StatusModule
{
    internal class ServiceStatusRequest : RequestBase
    {
        public override bool Validate()
        {
            return true;
        }
    }
}
