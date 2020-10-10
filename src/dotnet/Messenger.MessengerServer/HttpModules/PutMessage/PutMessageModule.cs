using EmbedIO;
using Messenger.Common.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.MessengerServer.HttpModules.PutMessage
{
    internal class PutMessageModule : ModuleBase<PutMessageRequest>
    {
        public override bool IsFinalHandler => throw new NotImplementedException();

        public PutMessageModule()
            :base(Routes.PUT_MESSAGE)
        {

        }

        protected override Task OnRequest(IHttpContext context, PutMessageRequest request)
        {
            
        }
    }
}
