using Amazon.SQS;
using ConsumerExample.Application.Services;
using ConsumerExample.Domain.Models;
using ConsumerExample.Domain.Repositories;
using ConsumerExample.Infrastructure.Repositories;
using ConsumerExample.Infrastructure.Services;
using ConsumerExample.Worker.Services;
using ConsumerExample.Worker.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ConsumerExample.Infrastructure.Configurations
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructureInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureLogger()
                .ConfigureAWSSQS()
                .ConfigureFeatureToggles()
                .ConfigureRepositories()
                .ConfigureQueueConsumer(configuration);

            return services;
        }

        public static IServiceCollection ConfigureLogger(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ILoggerService<>), typeof(LoggerService<>));
            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBloqueioRepository, FakeRepository>();
            return services;
        }

        public static IServiceCollection ConfigureFeatureToggles(this IServiceCollection services)
        {
            services.AddTransient<IFeatureToggleProvider, FeatureToggleProvider>();
            return services;
        }

        public static IServiceCollection ConfigureAWSSQS(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonSQS>();
            return services;
        }

        public static IServiceCollection ConfigureQueueConsumer(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddSingleton<IQueueConsumerService<ProcessarBloqueioUseCase, SolicitacaoBloqueioRequest>>(serviceProvider =>
               {

                   var sqsClient = serviceProvider
                   .GetRequiredService<IAmazonSQS>();

                   var scopeFactory = serviceProvider
                   .GetRequiredService<IServiceScopeFactory>();

                   var featureToggleProvider = serviceProvider
                   .GetRequiredService<IFeatureToggleProvider>();

                   var logger = serviceProvider
                   .GetRequiredService<ILoggerService<QueueConsumerService<ProcessarBloqueioUseCase, SolicitacaoBloqueioRequest>>>();

                   var queueConfig = configuration.GetSection("QueueConfiguration").GetSection("QueueBloqueio");

                   var queueConfiguration = new QueueConfigurationModel
                   {
                       Journey = queueConfig["Journey"] ?? "",
                       MaxNumberOfMessages = int.Parse(queueConfig["MaxNumberOfMessages"] ?? "10"),
                       QueueUrl = queueConfig["QueueUrl"] ?? "",
                       WaitTimeSeconds = int.Parse(queueConfig["WaitTimeSeconds"] ?? "20")
                   };

                   return new QueueConsumerService<ProcessarBloqueioUseCase, SolicitacaoBloqueioRequest>(sqsClient, scopeFactory, logger, featureToggleProvider, queueConfiguration);
               });

            return services;
        }
    }
}
