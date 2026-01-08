using ComplexSQSConsumerWorker.Entities.Enums;
using ComplexSQSConsumerWorker.Messages;

namespace ComplexSQSConsumerWorker.Events
{
    public class CreditProcessedEvent : Message
    {
        public Guid OperationId { get; set; }
        public decimal Amount { get; set; }
        public string CorrelationId { get; set; }
        public string TraceId { get; set; }
        public CreditStatus Status { get; set; }
    }
}
