using ComplexSQSConsumerWorker.Events;
using SQSConsumerWorker.Domain;
using SQSConsumerWorker.Handlers;
using SQSConsumerWorker.UseCases;

namespace SQSConsumerWorker.Workers
{
    public static class Configurations
    {
        public static void AdicionarDependencias(this IServiceCollection services)
        {
            services.AddScoped<IMessageHandler<CreditValueCommand>, SendCreditHandler>();
            services.AddScoped<IMessageHandler<CreditProcessedEvent>, ProcessCreditStatusHandler>();
        }

        public static void AddCommandConsumer(this IServiceCollection services)
        {
            services.AddHostedService(provider =>
                new ConsumerBuilder<CreditValueCommand>()
                   .ConfigureQueue("https://sqs.us-east-1.amazonaws.com/123456789012/fila")
                   .AddHandler<SendCreditHandler>()
                   .Build(provider)
            );
        }

        public static IServiceCollection AddEventConsumer(this IServiceCollection services)
        {
            services.AddHostedService(provider =>
                new ConsumerBuilder<CreditProcessedEvent>()
                    .ConfigureQueue("https://sqs.us-east-1.amazonaws.com/123456789012/fila")
                    .AddHandler<ProcessCreditStatusHandler>()
                    .Build(provider)
             );

            return services;
        }
    }
}
