using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Commands
{
    public abstract class BaseCommand : Message
    {
        public string Type { get; set; }
    }
}
