using ComplexSQSConsumerWorker.Infrastructure.Contracts;
using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Infrastructure
{
    public interface IHostedConsumer<TMessage> : IHostedService where TMessage : Message
    {
    }

    public class HostedConsumer<TMessage> : IHostedConsumer<TMessage> where TMessage : Message
    {
        private readonly IQueueConsumer<TMessage> _queueConsumer;

        public HostedConsumer(IQueueConsumer<TMessage> queueConsumer)
        {
            _queueConsumer = queueConsumer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _queueConsumer.StartConsumingAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _queueConsumer.StopConsumingAsync(cancellationToken);
        }
    }
}
