using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Handlers.Contract
{
    public interface IMessageHandler<T> where T : Message
    {
        Task<bool> HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
