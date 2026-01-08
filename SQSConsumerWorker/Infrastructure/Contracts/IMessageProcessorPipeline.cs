using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Infrastructure.Contracts
{
    public interface IMessageProcessorPipeline<TMessage> where TMessage : Message
    {
        Task ExecutePipelineAsync(TMessage message, CancellationToken ct);
    }
}
