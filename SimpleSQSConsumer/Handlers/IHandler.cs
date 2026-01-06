namespace SimpleSQSConsumer.Handlers
{
    public interface IHandler<T>
    {
        Task<bool> HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
