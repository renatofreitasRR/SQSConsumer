using ComplexSQSConsumerWorker.Messages;
using SQSConsumerWorker.Handlers;
using SQSConsumerWorker.Services;

public class ConsumerBuilder<T> where T : Message
{
    private string? _queueUrl;
    private readonly List<Type> _handlerTypes = new();

    public ConsumerBuilder<T> ConfigureQueue(string queueUrl)
    {
        _queueUrl = queueUrl;
        return this;
    }

    public ConsumerBuilder<T> AddHandler<TH>() where TH : IMessageHandler<T>
    {
        _handlerTypes.Add(typeof(TH));
        return this;
    }

    public IHostedService Build(IServiceProvider provider)
    {
        if (string.IsNullOrEmpty(_queueUrl))
            throw new InvalidOperationException("QueueUrl não configurada");

        if (!_handlerTypes.Any())
            throw new InvalidOperationException("Nenhum handler configurado");

        // resolve handlers via DI
        var handlers = _handlerTypes
            .Select(ht => (IMessageHandler<T>)provider.GetRequiredService(ht))
            .ToList();

        var chainHandler = new ChainMessageHandler<T>(handlers);

        var queueConsumer = provider.GetRequiredService<IQueueConsumer<T>>();

        if (queueConsumer is IConfigurableQueueConsumer configurable)
            configurable.Configure(_queueUrl);

        return new GenericBackgroundConsumer<T>(queueConsumer);
    }
}
