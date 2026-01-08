namespace ComplexSQSConsumerWorker.Middlewares.Contracts
{
    public interface IMessageMiddleware<TMessage> where TMessage : class
    {
        Task InvokeAsync(
        TMessage message,
        CancellationToken ct,
        Func<TMessage, CancellationToken, Task> next);
    }
}
