using Amazon.SQS;
using SimpleSQSConsumer.Domain;
using SimpleSQSConsumer.Handlers;
using SimpleSQSConsumer.Services;

namespace SimpleSQSConsumer.Configuration
{
    public static class AddConfigurations
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var awsOptions = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);

            services.AddAWSService<IAmazonSQS>();

            services.AddTransient<IQueueService, SQSQueueService>();

            services.AddTransient<IHandler<MovimentosEntradaMessage>, MovimentosEntradaHandler>();
            services.AddTransient<IHandler<MovimentosRespostaMessage>, MovimentosRespostaHandler>();

            services.AddHostedService<WorkerEntrada>();
            services.AddHostedService<WorkerResposta>();

            return services;
        }
    }
}
