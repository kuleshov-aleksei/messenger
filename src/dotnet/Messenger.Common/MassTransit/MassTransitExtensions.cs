using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Messenger.Common.Settings;

namespace Messenger.Common.MassTransit
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitConnection(this IServiceCollection services)
        {
            string rabbitAddress = DBSettings.ReadSettings("rabbit_mq_address");
            string rabbitUser = DBSettings.ReadSettings("rabbit_mq_user");
            string rabbitPassword = DBSettings.ReadSettings("rabbit_mq_password");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitAddress, "/", h =>
                    {
                        h.Username(rabbitUser);
                        h.Password(rabbitPassword);
                    });
                });
            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
