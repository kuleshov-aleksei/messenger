using HeyRed.Mime;
using Messenger.Common.JWT;
using Messenger.Common.Models.Fileserver;
using Messenger.Fileserver.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Messenger.Fileserver.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/fileserver")]
    public class FileController : ControllerBase
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private readonly MinioClient m_minioClient;
        private const string ROOT_BUCKET_NAME = "calamity";

        public FileController(MinioClient minioClient)
        {
            m_minioClient = minioClient;
        }

        // Url like https://messenger.local.encamy.com/fileserver/sharable_url?objectname=20220423/362c7dca-a108-4f16-b77e-5f7210ab3785.png
        // will redirect to https://calamity-s3.local.encamy.com/calamity/20220423/362c7dca-a108-4f16-b77e-5f7210ab3785.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=KDO0FK4OV3H4C134Y4XC%2F20220423%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20220423T164550Z&X-Amz-Expires=604800&X-Amz-SignedHeaders=host&X-Amz-Signature=bd085494e8652d4630701da70da3383c5b61d9a292aa488396a2f351cca62e8f
        // So this get query can be embeded in html like <img src="https://messenger.local.encamy.com/fileserver/sharable_url?objectname=20220423/362c7dca-a108-4f16-b77e-5f7210ab3785.png"/>
        // Btw. Auth done with cookies
        [HttpGet("sharable_url")]
        public async Task<IActionResult> GetSharableUrl([FromQuery] string objectname)
        {
            if (string.IsNullOrEmpty(objectname))
            {
                return BadRequest();
            }

            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Info("User {UserId} requested image {ObjectName}", userId, objectname);

            string presignedUrl = await m_minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(ROOT_BUCKET_NAME)
                .WithObject(objectname)
                .WithExpiry(604800)
            );

            return Redirect(presignedUrl);
        }

        // Do not change argument from "file"! It should be same in request and this handler
        [HttpPost("send")]
        public async Task<IActionResult> Send(List<IFormFile> file)
        {
            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Info("User {UserId} send {FileCount} file(s)", userId, file.Count);

            //TODO: add validation
            long size = file.Sum(f => f.Length);

            DateOnly dateOnly = DateOnly.FromDateTime(DateTime.UtcNow);
            string currentBucket = $"{dateOnly.Year:D4}{dateOnly.Month:D2}{dateOnly.Day:D2}";
            await CreateBucketIfNotExists(ROOT_BUCKET_NAME);

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile formFile in file)
            {
                if (!string.IsNullOrEmpty(formFile.ContentType) && formFile.Length > 0)
                {
                    string extension = MimeTypesMap.GetExtension(formFile.ContentType);
                    if (string.IsNullOrEmpty(extension) || extension == "bin")
                    {
                        continue;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(stream);
                        stream.Seek(0, SeekOrigin.Begin);

                        string generatedFilepath = currentBucket + "/" + Guid.NewGuid().ToString() + "." + extension;
                        uploadedFiles.Add(generatedFilepath);

                        m_logger.Info("Uploading file {OriginalFilename} to {Destination}", formFile.FileName, generatedFilepath);
                        await m_minioClient.PutObjectAsync(new PutObjectArgs()
                            .WithBucket(ROOT_BUCKET_NAME)
                            .WithContentType(formFile.ContentType)
                            .WithObject(generatedFilepath)
                            .WithStreamData(stream)
                            .WithObjectSize(formFile.Length)
                        );
                    }
                }
            }

            return Ok(new UploadFileResponseModel
            {
                Files = uploadedFiles,
            });
        }

        private async Task CreateBucketIfNotExists(string bucketName)
        {
            bool exists = await m_minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (!exists)
            {
                await m_minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }
        }
    }
}
