using SimpleSQSConsumer.Domain;

namespace SimpleSQSConsumer.Handlers
{
    public class ProcessStartEventsHandler : IHandler<QueueStartMessage>
    {
      
        public async Task<bool> HandleAsync(QueueStartMessage message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
