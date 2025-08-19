namespace SQSConsumerWorker.Services
{
    public interface IQueueConsumer<T>
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);
    }
}
