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
using Microsoft.Extensions.Logging;

namespace ConsumerExample.Infrastructure.Configurations
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructureInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureAWSSQS()
                .ConfigureFeatureToggles()
                .ConfigureRepositories()
                .ConfigureQueueConsumer(configuration);

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
               .AddSingleton<IQueueConsumerService<ProcessarBloqueioUseCase, SolicitaoBloqueioRequest>>(serviceProvider =>
               {

                   var sqsClient = serviceProvider
                   .GetRequiredService<IAmazonSQS>();

                   var scopeFactory = serviceProvider
                   .GetRequiredService<IServiceScopeFactory>();

                   var featureToggleProvider = serviceProvider
                   .GetRequiredService<IFeatureToggleProvider>();

                   var logger = serviceProvider
                   .GetRequiredService<ILogger<QueueConsumerService<ProcessarBloqueioUseCase, SolicitaoBloqueioRequest>>>();

                   var queueConfig = configuration.GetSection("QueuConfiguration").GetSection("QueueBloqueio");

                   var queueConfiguration = new QueueConfigurationModel
                   {
                       Journey = queueConfig.GetSection(nameof(QueueConfigurationModel.Journey)).Value,
                       MaxNumberOfMessages = int.Parse(queueConfig.GetSection(nameof(QueueConfigurationModel.MaxNumberOfMessages)).Value ?? "10"),
                       QueueUrl = queueConfig.GetSection(nameof(QueueConfigurationModel.QueueUrl)).Value,
                       WaitTimeSeconds = int.Parse(queueConfig.GetSection(nameof(QueueConfigurationModel.WaitTimeSeconds)).Value ?? "20")
                   };

                   return new QueueConsumerService<ProcessarBloqueioUseCase, SolicitaoBloqueioRequest>(sqsClient, scopeFactory, logger, featureToggleProvider, queueConfiguration);
               });

            return services;
        }
    }
}
