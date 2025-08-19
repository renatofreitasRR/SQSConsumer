using SQSConsumerWorker.Services;

public class GenericBackgroundConsumer<T> : BackgroundService
{
    private readonly IQueueConsumer<T> _queueConsumer;

    public GenericBackgroundConsumer(IQueueConsumer<T> queueConsumer)
    {
        _queueConsumer = queueConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _queueConsumer.StartConsumingAsync(stoppingToken);
    }
}