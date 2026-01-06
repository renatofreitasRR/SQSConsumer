using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Events
{
    public abstract class BaseEvent : Message
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
    }
}
