using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Infrastructure
{
    public class HostedConsumer<T> : IHostedService where T : Message
    {
        private readonly IHostedService _innerService;
        public HostedConsumer(IServiceProvider serviceProvider, Func<IServiceProvider, IHostedService> factory)
        {
            _innerService = factory(serviceProvider);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _innerService.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _innerService.StopAsync(cancellationToken);
        }
    }
}
