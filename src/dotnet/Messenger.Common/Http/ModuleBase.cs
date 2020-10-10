using EmbedIO;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Common.Http
{
    public abstract class ModuleBase<T> : WebModuleBase where T : RequestBase, IRequest
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();

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

        protected override async Task OnRequestAsync(IHttpContext context)
        {
            if (!TryGetRequestString(context.Request, out string requestString))
            {
                return;
            }

            try
            {
                T request = JsonConvert.DeserializeObject<T>(requestString);
                if (!request.Validate())
                {
                    m_logger.Error("Invalid request");
                    await SendResponse(context, HttpStatusCode.BadRequest, new ServerError("Invalid request"));
                }

                await OnRequest(context, request);
            }
            catch (Exception e)
            {
                m_logger.Error(e, "Failed to process request");
                await SendResponse(context, HttpStatusCode.InternalServerError);
                return;
            }
        }

        protected abstract Task OnRequest(IHttpContext context, T request);

        public static Task SendResponse(IHttpContext context, HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.StatusDescription = statusCode.ToString();

            return Task.CompletedTask;
        }

        public static async Task SendResponse(IHttpContext context, HttpStatusCode statusCode, string json)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.StatusDescription = statusCode.ToString();
            context.Response.ContentType = "application/json";

            using (TextWriter textWriter = context.OpenResponseText(new UTF8Encoding()))
            {
                await textWriter.WriteAsync(json);
            }
        }

        public static async Task SendResponse(IHttpContext context, HttpStatusCode statusCode, object obj)
        {
            await SendResponse(context, statusCode, JsonConvert.SerializeObject(obj));
        }

        public static async Task SendResponse(IHttpContext context, HttpStatusCode statusCode, ServerResponse serverResponse)
        {
            await SendResponse(context, statusCode, JsonConvert.SerializeObject(serverResponse));
        }

        public static async Task SendResponse(IHttpContext context, HttpStatusCode statusCode, ServerError serverError)
        {
            await SendResponse(context, statusCode, JsonConvert.SerializeObject(serverError));
        }
    }
}
