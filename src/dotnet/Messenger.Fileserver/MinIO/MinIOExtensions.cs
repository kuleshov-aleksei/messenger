using Messenger.Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Minio.AspNetCore;

namespace Messenger.Fileserver.MinIO
{
    public static class MinIOExtensions
    {
        public static IServiceCollection AddMinIOConfiguration(this IServiceCollection services)
        {
            string endpoint = DBSettings.ReadSettings("s3_endpoint");
            string accessKey = DBSettings.ReadSettings("s3_access_key");
            string secretKey = DBSettings.ReadSettings("s3_secret_key");

            services.AddMinio(options =>
            {
                options.Endpoint = endpoint;

                options.ConfigureClient(client =>
                {
                    client.WithCredentials(accessKey, secretKey);
                    client.WithSSL();
                });
            });

            return services;
        }
    }
}
