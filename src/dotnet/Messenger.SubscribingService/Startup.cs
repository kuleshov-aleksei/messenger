using Invio.Extensions.Authentication.JwtBearer;
using MassTransit;
using Messenger.Common.JWT;
using Messenger.Common.MassTransit;
using Messenger.Common.Redis;
using Messenger.SubscribingService.Consumers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Messenger.SubscribingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRedisConnection();
            services.AddMassTransitConnection(
                additionConfiguration: (x) =>
                {
                    x.AddConsumer<IMConsumer>();
                },
                additionalRabbitConfiguration: (context, cfg) =>
                {
                    cfg.ReceiveEndpoint(new TemporaryEndpointDefinition(), e =>
                    {
                        e.Durable = false;
                        e.ConfigureConsumeTopology = false;
                        e.ConfigureConsumer<IMConsumer>(context);
                        e.Bind("incoming-messages");
                    });
                });
            services.AddScoped<WebsocketConnectionHandler>();
            services.AddSingleton<MessageHub>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.AddQueryStringAuthentication();

                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.MapInboundClaims = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = JwtHelper.LoadSecurityKey(),
                    ValidateIssuer = true,
                    ValidIssuer = JwtHelper.JWT_ISSUER,
                    ValidateAudience = true,
                    ValidAudiences = new List<string>
                    {
                        JwtHelper.JWT_WEB_AUDIENCE,
                    },
                    ClockSkew = TimeSpan.FromMinutes(1),
                };
            });


            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.None;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
                    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Error;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.None;
                    options.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Template.WebAPI", Version = "v1" });
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Template.WebAPI v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            WebSocketOptions websocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(2),
            };
            websocketOptions.AllowedOrigins.Add("*");

            app.UseWebSockets(websocketOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
