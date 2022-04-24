using Messenger.Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace Messenger.Common.Redis
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedisConnection(this IServiceCollection services)
        {
            string host = DBSettings.ReadSettings("redis_address");
            int port = DBSettings.ReadSettings<int>("redis_port");

#if DEBUG
            host = "192.168.40.43";
#endif

            RedisConfiguration redisConfiguration = new RedisConfiguration()
            {
                AbortOnConnectFail = true,
                Hosts = new RedisHost[]
                {
                    new RedisHost()
                    {
                        Host = host,
                        Port = port,
                    }
                },
                ConnectTimeout = 3000,
                MaxValueLength = 1024,
	            PoolSize = 5,
            };
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

            return services;
        }
    }
}
