using EmbedIO;
using Messenger.Common.JWT;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Common.Http
{
    public abstract class ModuleBase<T> : WebModuleBase where T : RequestBase, IRequest
    {
        private Logger m_logger = LogManager.GetCurrentClassLogger();
        private JwtHelper m_jwtHelper;
        protected bool NeedAuthorization { get; set; } = true;

        protected ModuleBase(string baseRoute, JwtHelper jwtHelper)
            : base(baseRoute)
        {
            m_jwtHelper = jwtHelper;
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
                int userId = 0;

                if (request != null)
                {
                    if (!request.Validate())
                    {
                        m_logger.Error("Invalid request");
                        await SendResponse(context, HttpStatusCode.BadRequest, new ServerError("Invalid request"));
                        return;
                    }

                    if (NeedAuthorization)
                    {
                        if (!request.CheckAuthorization(m_jwtHelper, request, out userId))
                        {
                            m_logger.Error("Authorization failed");
                            await SendResponse(context, HttpStatusCode.Unauthorized);
                            return;
                        }

                        if (userId == 0)
                        {
                            m_logger.Error("Authorization failed");
                            await SendResponse(context, HttpStatusCode.Unauthorized);
                            return;
                        }
                    }
                }

                await OnRequest(context, request, userId);
            }
            catch (Exception e)
            {
                m_logger.Error("Failed to process request: " + e.Message);
                await SendResponse(context, HttpStatusCode.InternalServerError);
                return;
            }
        }

        protected abstract Task OnRequest(IHttpContext context, T request, int userId);

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
        
        public static async Task SendResponse(IHttpContext context, HttpStatusCode statusCode, ResponseBase response)
        {
            await SendResponse(context, statusCode, response.ToJson());
        }
    }
}
