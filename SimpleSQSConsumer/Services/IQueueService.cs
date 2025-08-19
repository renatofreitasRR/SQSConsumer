using SimpleSQSConsumer.Handlers;

namespace SimpleSQSConsumer.Services
{
    public interface IQueueService
    {
        Task ConsumeQueueAsync<T>(string queueUrl, IHandler<T> handler, CancellationToken cancellationToken);
    }
}
