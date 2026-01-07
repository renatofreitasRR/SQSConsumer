using Amazon.SQS;
using ComplexSQSConsumerWorker.Events;
using ComplexSQSConsumerWorker.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SQSConsumerWorker.Domain;
using SQSConsumerWorker.Handlers;
using SQSConsumerWorker.Services;

namespace ComplexSQSConsumerWorker.Configuration
{
    public static class Configurations
    {
        public static void AddConsumerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMessageHandler<CreditValueCommand>, SendCreditHandler>();
            services.AddTransient<IMessageHandler<CreditProcessedEvent>, ProcessCreditStatusHandler>();
            services.AddTransient(typeof(IQueueConsumer<>), typeof(SqsQueueConsumer<>));

            services.AddDefaultAWSOptions(
                configuration.GetAWSOptions()
            );
            services.AddAWSService<IAmazonSQS>();

            services.AddConsumers(configuration);
        }

        public static void AddSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<QueueSettings>(configuration.GetSection(nameof(QueueSettings)));
        }

        public static void AddConsumers(this IServiceCollection services, IConfiguration configuration)
        {
            var queueSettings = configuration
            .GetSection(nameof(QueueSettings))
            .Get<QueueSettings>();

            if(queueSettings == null)
                throw new InvalidOperationException("QueueSettings is not provided");

            services.AddConsumer<CreditValueCommand>(provider =>
                new ConsumerBuilder<CreditValueCommand>()
                   .ConfigureQueue(queueSettings.CommandQueueUrl)
                   .AddHandler<IMessageHandler<CreditValueCommand>>()
                   .Build(provider)
            );

            services.AddConsumer<CreditProcessedEvent>(provider =>
               new ConsumerBuilder<CreditProcessedEvent>()
                   .ConfigureQueue(queueSettings.EventQueueUrl)
                   .AddHandler<IMessageHandler<CreditProcessedEvent>>()
                   .Build(provider)
            );
        }
    }
}
