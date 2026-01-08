using ComplexSQSConsumerWorker.Infrastructure;
using ComplexSQSConsumerWorker.Infrastructure.Contracts;
using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Extensions
{
    public static class ConsumerRegistrationExtension
    {
        public static void AddConsumer<TMessage>(this IServiceCollection services, Action<ConsumerBuilder<TMessage>> builder) where TMessage : Message
        {
            services.AddScoped<IMessageProcessorPipeline<TMessage>, MessageProcessorPipeline<TMessage>>();

            builder.Invoke(new ConsumerBuilder<TMessage>(services));
        }

    }
}
