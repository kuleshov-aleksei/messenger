using EmbedIO;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Common.Http
{
    internal abstract class ModuleBase : WebModuleBase
    {
        protected ModuleBase(string baseRoute)
            : base(baseRoute)
        {
        }

        protected bool TryGetRequestString(IHttpRequest request, out string stringRequest)
        {
            MemoryStream memoryStream = new MemoryStream();

            while (true)
            {
                byte[] localBuffer = new byte[8192];
                int bytesReaded = request.InputStream.Read(localBuffer, 0, localBuffer.Length);
                if (bytesReaded == 0)
                {
                    break;
                }

                memoryStream.Write(localBuffer, 0, bytesReaded);
            }

            stringRequest = Encoding.Default.GetString(memoryStream.ToArray());
            return true;
        }

        internal static Task SendResponse(IHttpContext context, HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.StatusDescription = statusCode.ToString();

            return Task.CompletedTask;
        }

        internal static async Task SendResponse(IHttpContext context, HttpStatusCode statusCode, string json)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.StatusDescription = statusCode.ToString();
            context.Response.ContentType = "application/json";

            using (TextWriter textWriter = context.OpenResponseText(new UTF8Encoding()))
            {
                await textWriter.WriteAsync(json);
            }
        }

        internal static async Task SendResponse(IHttpContext context, HttpStatusCode statusCode, object obj)
        {
            await SendResponse(context, statusCode, JsonConvert.SerializeObject(obj));
        }

        internal static async Task SendResponse(IHttpContext context, HttpStatusCode statusCode, ServerResponse serverResponse)
        {
            await SendResponse(context, statusCode, JsonConvert.SerializeObject(serverResponse));
        }
    }
}
