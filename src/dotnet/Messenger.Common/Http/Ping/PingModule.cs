using EmbedIO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.Common.Http.Ping
{
    internal class PingModule : ModuleBase<PingRequest>
    {
        public PingModule()
            : base(Routes.PING_ROUTE, null)
        {
            base.NeedAuthorization = false;
        }

        public override bool IsFinalHandler => true;

        protected override async Task OnRequest(IHttpContext context, PingRequest request, int userId)
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
