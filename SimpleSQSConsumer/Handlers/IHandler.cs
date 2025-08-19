namespace SimpleSQSConsumer.Handlers
{
    public interface IHandler<T>
    {
        Task HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
