using Aws.Int.Infraestructure.Logging;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using StackExchange.Redis.Extensions.Core.Configuration;
using System.Collections.Generic;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Aws.Int.Infrastructure.SNS;
using Aws.Int.Domain.Interfaces;
using Aws.Int.Infrastructure.SecretManager;

namespace Aws.Int.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            IConfigurationRoot configuration = GetConfiguration();
            services.AddSingleton<IConfiguration>(configuration);

#if DEBUG
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
#endif
            services.AddMediatR(AppDomain.CurrentDomain.Load("Aws.Int.Application"));
            services.AddAutoMapper(typeof(Function).Assembly);
            services.AddSingleton<ILogger, Logger>();

            services.AddHttpClient();

            services.AddSingleton<ISnsClient, SnsClient>();
            services.AddSingleton<IAwsSecretManagerService, AwsSecretManagerService>();

            return services;
        }

        public static void AddRedis(this IServiceCollection services,
            string hostsInLine,
            string password,
            bool abortOnConnectFail = true,
            int syncTimeout = 30)
        {
            var newRedisConfiguration = new RedisConfiguration()
            {
                AbortOnConnectFail = abortOnConnectFail,
                Password = password,
                Ssl = true
            };

            if (!string.IsNullOrEmpty(hostsInLine))
            {
                var hosts = new List<RedisHost>();

                var splitted = hostsInLine.Split(' ');
                for (int i = 0; i < splitted.Length - 1; i += 2)
                {
                    var host = new RedisHost()
                    {
                        Host = splitted[i],
                        Port = Convert.ToInt32(splitted[i + 1])
                    };

                    hosts.Add(host);
                }

                newRedisConfiguration.Hosts = hosts.ToArray();
            }

            newRedisConfiguration.ConfigurationOptions.SyncTimeout = Convert.ToInt32(syncTimeout);

            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(newRedisConfiguration);
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile($"appsettings.json")
                            .AddEnvironmentVariables();

            var configuration = builder.Build();
            return configuration;
        }
    }
}
