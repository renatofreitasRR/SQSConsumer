using ComplexSQSConsumerWorker.Messages;
using ComplexSQSConsumerWorker.Middlewares.Contracts;
using Microsoft.AspNetCore.Http;

namespace ComplexSQSConsumerWorker.Middlewares
{
    public class AddTraceMiddleware<TMessage>: IMessageMiddleware<TMessage> where TMessage : Message
    {
        private readonly RequestDelegate _next;

        public AddTraceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TMessage message)
        {
            context.Items["TraceId"] = Guid.NewGuid().ToString();
            await _next(context);
        }
    }
}
