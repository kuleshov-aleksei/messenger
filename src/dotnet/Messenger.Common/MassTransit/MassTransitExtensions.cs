using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Messenger.Common.Settings;
using System.Collections.Generic;
using System;
using MassTransit.RabbitMqTransport;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace Messenger.Common.MassTransit
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitConnection(this IServiceCollection services,
            Action<IServiceCollectionBusConfigurator> additionConfiguration = null,
            Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> additionalRabbitConfiguration = null)
        {
            string rabbitAddress = DBSettings.ReadSettings("rabbit_mq_address");
            string rabbitUser = DBSettings.ReadSettings("rabbit_mq_user");
            string rabbitPassword = DBSettings.ReadSettings("rabbit_mq_password");

            services.AddMassTransit(x =>
            {
                additionConfiguration?.Invoke(x);

                x.UsingRabbitMq((context, cfg) =>
                {
                    additionalRabbitConfiguration?.Invoke(context, cfg);

                    cfg.Host(rabbitAddress, "/", h =>
                    {
                        h.Username(rabbitUser);
                        h.Password(rabbitPassword);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
