using Amazon.SQS;
using ComplexSQSConsumerWorker.Events;
using ComplexSQSConsumerWorker.Extensions;
using SQSConsumerWorker.Domain;
using SQSConsumerWorker.Handlers;

namespace ComplexSQSConsumerWorker.Configuration
{
    public static class Configurations
    {
        public static void AddConsumerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(
                configuration.GetAWSOptions()
            );

            services.AddAWSService<IAmazonSQS>();

            services.AddConsumers(configuration);
        }

        public static void AddConsumers(this IServiceCollection services, IConfiguration configuration)
        {
            var queueSettings = configuration
            .GetSection(nameof(QueueSettings))
            .Get<QueueSettings>();

            if(queueSettings == null)
                throw new InvalidOperationException("QueueSettings is not provided");

            services.AddConsumer<CreditProcessedEvent>(config =>
                config
                .ConfigureQueue(queueSettings.EventQueueUrl)
                .AddHandler<ProcessCreditStatusHandler>()
                .Build()
            );

            services.AddConsumer<CreditValueCommand>(config =>
                config
                .ConfigureQueue(queueSettings.CommandQueueUrl)
                .AddHandler<SendCreditHandler>()
                .Build()
            );
        }
    }
}
