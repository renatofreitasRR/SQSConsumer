using ComplexSQSConsumerWorker.Infrastructure;
using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Extensions
{
    public static class ConsumerRegistrationExtensions
    {
        public static void AddConsumer<TMessage>(this IServiceCollection services, Action<ConsumerBuilder<TMessage>> builder) where TMessage : Message
        {
            builder.Invoke(new ConsumerBuilder<TMessage>(services));
        }
    }
}
