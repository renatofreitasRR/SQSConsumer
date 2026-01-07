using ComplexSQSConsumerWorker.Infrastructure;
using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Extensions
{
    public static class ConsumerRegistrationExtensions
    {
        public static void AddConsumer<T>(this IServiceCollection services, Func<IServiceProvider, IHostedService> factory) where T : Message
        {
            services.AddHostedService(provider => new HostedConsumer<T>(provider, factory));
        }
    }
}
