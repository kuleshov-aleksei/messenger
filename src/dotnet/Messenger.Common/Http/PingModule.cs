using EmbedIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Common.Http
{
    internal class PingModule : ModuleBase
    {
        public PingModule()
            : base(Routes.PING_ROUTE)
        {
        }

        public override bool IsFinalHandler => true;

        protected override async Task OnRequestAsync(IHttpContext context)
        {
            await SendResponse(context, HttpStatusCode.OK, new ServerResponse("pong"));
        }
    }
}
