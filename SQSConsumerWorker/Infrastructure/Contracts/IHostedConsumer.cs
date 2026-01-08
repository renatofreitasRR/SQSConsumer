using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Infrastructure.Contracts
{
    public interface IHostedConsumer<TMessage> : IHostedService where TMessage : Message
    {
    }
}
