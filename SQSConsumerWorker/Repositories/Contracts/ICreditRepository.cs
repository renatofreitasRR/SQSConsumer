using ComplexSQSConsumerWorker.Entities;

namespace ComplexSQSConsumerWorker.Repositories.Contracts
{
    public interface ICreditRepository
    {
        Task<bool> SaveCredit(CreditEntity credit, CancellationToken cancellationToken);
    }
}
