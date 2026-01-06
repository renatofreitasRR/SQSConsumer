using ComplexSQSConsumerWorker.Messages;

namespace SQSConsumerWorker.Handlers
{
    public interface IMessageHandler<T> where T : Message
    {
        Task<bool> HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
