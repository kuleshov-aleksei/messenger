using HeyRed.Mime;
using MassTransit;
using Messenger.Common;
using Messenger.Common.JWT;
using Messenger.Common.MassTransit.Models;
using Messenger.Common.Models.Fileserver;
using Messenger.Common.MySql;
using Messenger.Common.Settings;
using Messenger.Common.Tools;
using Messenger.InstantMessagingService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Messenger.InstantMessagingService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/messenger/instant")]
    public class InstantMessagesController : ControllerBase
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private readonly IRedisDatabase m_redis;
        private readonly ISendEndpointProvider m_sendEndpointProvider;
        private readonly HttpClient m_httpClient;

        public InstantMessagesController(IRedisDatabase redisDatabase, ISendEndpointProvider sendEndpointProvider, HttpClient httpClient)
        {
            m_redis = redisDatabase;
            m_sendEndpointProvider = sendEndpointProvider;
            m_httpClient = httpClient;
        }

        [HttpPost("send")]
        public async Task SendMessage(SendMessageRequest request)
        {
            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Trace($"User {userId} is sending message to chat {request.ChatId}");

            List<int> userChats = await GlobalSettings.Instance.Database.GetUserChatListAsync(m_redis, userId);
            if (!userChats.Contains(request.ChatId))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            if (string.IsNullOrEmpty(request.Message))
            {
                m_logger.Debug("Message is empty. Rejecting");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            ISendEndpoint endpoint = await m_sendEndpointProvider.GetSendEndpoint(new Uri("queue:incoming-messages"));

            m_logger.Trace($"Sending message to RMQ");
            await endpoint.Send(new NewMessage
            {
                ChatId = request.ChatId,
                Message = request.Message,
                UserId = userId,
                MessageTime = UnixEpochTools.ToEpoch(DateTime.UtcNow),
            });
        }

        [HttpPost("send_embed")]
        public async Task<IActionResult> SendEmbed([FromForm] IFormFile file, [FromForm] string description, [FromForm] int chatid)
        {
            if (chatid < 1 || file == null)
            {
                return BadRequest();
            }

            long messageTime = UnixEpochTools.ToEpoch(DateTime.UtcNow);
            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Info("User {UserId} send message with file to chat {ChatId}", userId, chatid);

            bool isImage = false;
            string uploadedFileUrl = string.Empty;
            using (MultipartFormDataContent multipartContent = new MultipartFormDataContent())
            {
                multipartContent.Headers.ContentType.MediaType = "multipart/form-data";
                multipartContent.Add(file, "file");
                string extension = MimeTypesMap.GetExtension(file.ContentType);
                if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "gif" || extension == "webp")
                {
                    isImage = true;
                }

                string firstAuthHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault();
                string token = firstAuthHeader.Replace("Bearer ", "");
                m_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string endpoint = DBSettings.ReadSettings("main_endpoint");
                Stopwatch stopwatch = Stopwatch.StartNew();
                HttpResponseMessage response = await m_httpClient.PostAsync($"{endpoint}/fileserver/send", multipartContent);
                stopwatch.Stop();
                m_logger.Info("Send file request done in {ElapsedMs}", stopwatch.ElapsedMilliseconds);
                if (!response.IsSuccessStatusCode)
                {
                    m_logger.Error($"Got failed status code from fileserver: {response.StatusCode} {response.ReasonPhrase}");
                    return BadRequest();
                }

                string responseJson = await response.Content.ReadAsStringAsync();
                UploadFileResponseModel responseModel = JsonConvert.DeserializeObject<UploadFileResponseModel>(responseJson);

                m_logger.Info($"Files uploaded as: {string.Join(", ", responseModel.Files)}");
                if (responseModel.Files == null || responseModel.Files.Count != 1)
                {
                    m_logger.Warn("Something went wrong. Exactly 1 file was expected. Got {GotFilesCount}", responseModel.Files?.Count);
                }
                else
                {
                    uploadedFileUrl = responseModel.Files.First();
                }
            }

            ISendEndpoint rmqEndpoint = await m_sendEndpointProvider.GetSendEndpoint(new Uri("queue:incoming-messages"));

            m_logger.Trace($"Sending message to RMQ");
            NewMessage message = new NewMessage
            {
                ChatId = chatid,
                Message = description,
                UserId = userId,
                MessageTime = messageTime,
            };

            if (isImage)
            {
                message.MessageImageUrl = uploadedFileUrl;
            }
            else
            {
                message.MessageAttachmentUrl = uploadedFileUrl;
            }

            await rmqEndpoint.Send(message);

            return Ok();
        }
    }
}
