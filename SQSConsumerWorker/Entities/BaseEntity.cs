using ComplexSQSConsumerWorker.Entities.Contracts;

namespace ComplexSQSConsumerWorker.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
