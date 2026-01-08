using ComplexSQSConsumerWorker.Entities.Enums;

namespace ComplexSQSConsumerWorker.Entities
{
    public class CreditEntity : BaseEntity
    {
        public Guid OperationId { get; set; }
        public decimal Amount { get; set; }
        public CreditStatus Status { get; set; }
        public DateTime DateToCredit { get; set; }
    }
}
