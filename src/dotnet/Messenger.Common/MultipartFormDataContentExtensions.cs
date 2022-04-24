using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Messenger.Common
{
    public static class MultipartFormDataContentExtensions
    {
        public static void Add(this MultipartFormDataContent content, IFormFile file, string name)
        {
            StreamContent stream = new StreamContent(file.OpenReadStream());
            stream.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(stream, name, file.FileName);
        }
    }
}
