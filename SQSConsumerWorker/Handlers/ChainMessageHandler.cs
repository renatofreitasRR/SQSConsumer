using SQSConsumerWorker.Domain;
using SQSConsumerWorker.Handlers;

public class ChainMessageHandler<T> : IMessageHandler<T>
{
    private readonly IEnumerable<IMessageHandler<T>> _handlers;

    public ChainMessageHandler(IEnumerable<IMessageHandler<T>> handlers)
    {
        _handlers = handlers;
    }


    public async Task<bool> HandleAsync(T message, CancellationToken cancellationToken = default)
    {
        foreach (var handler in _handlers)
        {
            if (await handler.HandleAsync(message, cancellationToken))
                return true;
        }
        return false;
    }

}
