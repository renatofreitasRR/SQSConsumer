using ComplexSQSConsumerWorker.Messages;
using ComplexSQSConsumerWorker.Middlewares.Contracts;

namespace ComplexSQSConsumerWorker.Middlewares
{
    public class TraceMiddleware<TMessage>: IMessageMiddleware<TMessage> where TMessage : Message
    {
        private readonly ILogger<TraceMiddleware<TMessage>> _logger;

        public TraceMiddleware(ILogger<TraceMiddleware<TMessage>> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(TMessage message, CancellationToken ct, Func<TMessage, CancellationToken, Task> next)
        {
            _logger.BeginScope(new Dictionary<string, object>
            {
                ["MessageType"] = typeof(TMessage).Name,
                ["TraceId"] = message.GetTraceId(),
                ["ContextId"] = message.GetContextId()
            });
            _logger.LogTrace("Starting processing message with ID: {MessageId}", typeof(TMessage).Name);

            await next(message, ct);

            _logger.LogTrace("Finished processing message with ID: {MessageId}", typeof(TMessage).Name);
        }
    }
}
