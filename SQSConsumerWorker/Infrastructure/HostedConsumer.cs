using ComplexSQSConsumerWorker.Infrastructure.Contracts;
using ComplexSQSConsumerWorker.Messages;
using System.Threading;

namespace ComplexSQSConsumerWorker.Infrastructure
{
   

    public class HostedConsumer<TMessage> : BackgroundService where TMessage : Message
    {
        private readonly IQueueConsumer<TMessage> _queueConsumer;

        public HostedConsumer(IQueueConsumer<TMessage> queueConsumer)
        {
            _queueConsumer = queueConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"Iniciando StartAsync para {typeof(TMessage).Name}");

            await _queueConsumer.StartConsumingAsync(stoppingToken);
        }
    }
}
