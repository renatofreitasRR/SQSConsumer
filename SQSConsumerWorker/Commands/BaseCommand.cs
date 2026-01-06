using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Commands
{
    public abstract class BaseCommand : Message
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
    }
}
