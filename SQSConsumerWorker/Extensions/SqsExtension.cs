using Amazon.SQS;
using ComplexSQSConsumerWorker.Configuration;
using ComplexSQSConsumerWorker.Events;
using ComplexSQSConsumerWorker.Middlewares;
using SQSConsumerWorker.Domain;
using SQSConsumerWorker.Handlers;

namespace ComplexSQSConsumerWorker.Extensions
{
    public static class SqsExtension
    {
        public static void AddSqsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(
                configuration.GetAWSOptions()
            );

            services.AddAWSService<IAmazonSQS>();
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
                .AddMiddleware<TraceMiddleware<CreditProcessedEvent>>()
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
