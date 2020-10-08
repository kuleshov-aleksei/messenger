using EmbedIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Common.Http.Ping
{
    internal class PingModule : ModuleBase<PingRequest>
    {
        public PingModule()
            : base(Routes.PING_ROUTE)
        {
        }

        public override bool IsFinalHandler => true;

        protected override async Task OnRequest(IHttpContext context, PingRequest request)
        {
            await SendResponse(context, HttpStatusCode.OK,
                JsonConvert.SerializeObject(
                new PingResponse
                {
                    Text = "pong"
                },
                Formatting.Indented)
            );
        }
    }
}
