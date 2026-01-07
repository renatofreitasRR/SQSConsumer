using ComplexSQSConsumerWorker.Events;

namespace SQSConsumerWorker.Handlers
{
    public class ProcessCreditStatusHandler : IMessageHandler<CreditProcessedEvent>
    {
        public async Task<bool> HandleAsync(CreditProcessedEvent @event, CancellationToken cancellationToken = default)
        {
            return true;
        }
    }
}
