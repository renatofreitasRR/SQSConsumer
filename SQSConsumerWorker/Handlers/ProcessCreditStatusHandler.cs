using ComplexSQSConsumerWorker.Events;
using SQSConsumerWorker.Handlers;

namespace SQSConsumerWorker.UseCases
{
    public class ProcessCreditStatusHandler : IMessageHandler<CreditProcessedEvent>
    {
        public Task<bool> HandleAsync(CreditProcessedEvent @event, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
