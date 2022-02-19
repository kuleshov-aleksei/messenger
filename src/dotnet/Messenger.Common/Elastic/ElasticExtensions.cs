using Messenger.Common.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Common.Elastic
{
    public static class ElasticExtensions
    {
        public static IServiceCollection AddElasticConnection(this IServiceCollection services)
        {
            services.AddSingleton<IdGenerator>();
            services.AddSingleton(ESClient.CreateElasticClient());
            services.AddSingleton<EsInteractor>();

            return services;
        }
    }
}
