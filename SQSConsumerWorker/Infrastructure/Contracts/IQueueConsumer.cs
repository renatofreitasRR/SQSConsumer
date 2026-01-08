namespace ComplexSQSConsumerWorker.Infrastructure.Contracts
{
    public interface IQueueConsumer<T>
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);
        Task StopConsumingAsync(CancellationToken cancellationToken);
    }
}
