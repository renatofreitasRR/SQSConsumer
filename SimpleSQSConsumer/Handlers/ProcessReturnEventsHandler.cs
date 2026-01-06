using SimpleSQSConsumer.Domain;

namespace SimpleSQSConsumer.Handlers
{
    public class ProcessReturnEventsHandler : IHandler<QueueReturnMessage>
    {
        public async Task<bool> HandleAsync(QueueReturnMessage message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
