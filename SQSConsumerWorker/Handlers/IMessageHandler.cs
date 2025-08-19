namespace SQSConsumerWorker.Handlers
{
    public interface IMessageHandler<T>
    {
        Task<bool> HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
