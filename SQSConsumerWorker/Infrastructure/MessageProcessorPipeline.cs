using ComplexSQSConsumerWorker.Handlers.Contract;
using ComplexSQSConsumerWorker.Infrastructure.Contracts;
using ComplexSQSConsumerWorker.Messages;
using ComplexSQSConsumerWorker.Middlewares.Contracts;

namespace ComplexSQSConsumerWorker.Infrastructure
{
    public class MessageProcessorPipeline<TMessage>: IMessageProcessorPipeline<TMessage> where TMessage : Message
    {
        private readonly IList<IMessageMiddleware<TMessage>> _middlewares;
        private readonly IMessageHandler<TMessage> _handler;

        public MessageProcessorPipeline(
            IEnumerable<IMessageMiddleware<TMessage>> middlewares,
            IMessageHandler<TMessage> handler)
        {
            _middlewares = middlewares.ToList();
            _handler = handler;
        }

        public Task ExecutePipelineAsync(TMessage message, CancellationToken ct)
        {
            Func<TMessage, CancellationToken, Task> handlerDelegate =
                (msg, token) => _handler.HandleAsync(msg, token);

            foreach (var middleware in _middlewares.Reverse())
            {
                var next = handlerDelegate;
                handlerDelegate = (msg, token) =>
                    middleware.InvokeAsync(msg, token, next);
            }

            return handlerDelegate(message, ct);
        }
    }
}
