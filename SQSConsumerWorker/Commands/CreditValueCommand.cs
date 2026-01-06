using ComplexSQSConsumerWorker.Commands;

namespace SQSConsumerWorker.Domain
{
    public class CreditValueCommand : BaseCommand
    {
        public Guid OperationId { get; set; }
        public decimal Amount { get; set; }
        public string CorrelationId { get; set; }
        public string TraceId { get; set; }
        public DateTime DateToCredit { get; set; }

    }
}
