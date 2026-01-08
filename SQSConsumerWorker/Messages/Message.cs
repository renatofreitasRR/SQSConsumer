namespace ComplexSQSConsumerWorker.Messages
{
    public abstract class Message
    {
        protected Guid Id { get; set; }
        protected string TraceId { get; set; }
        protected string ContextId { get; set; }

        public string GetContextId() => ContextId;
        public string GetTraceId() => ContextId;
    }
}
