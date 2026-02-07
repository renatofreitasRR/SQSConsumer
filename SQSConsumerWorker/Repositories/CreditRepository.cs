using ComplexSQSConsumerWorker.Entities;
using ComplexSQSConsumerWorker.Repositories.Contracts;

namespace ComplexSQSConsumerWorker.Repositories
{
    public class CreditRepository : ICreditRepository
    {
        private IDictionary<Guid, CreditEntity> _credits = new Dictionary<Guid, CreditEntity>();

        public async Task<bool> SaveCredit(CreditEntity credit, CancellationToken cancellationToken)
        {
            _credits[Guid.NewGuid()] = credit;

            await Task.Delay(50);

            return true;
        }
    }
}
