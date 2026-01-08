using ComplexSQSConsumerWorker.Entities;
using ComplexSQSConsumerWorker.Entities.Enums;
using ComplexSQSConsumerWorker.Handlers.Contract;
using ComplexSQSConsumerWorker.Repositories.Contracts;
using SQSConsumerWorker.Domain;

namespace SQSConsumerWorker.Handlers
{
    public class SendCreditHandler : IMessageHandler<CreditValueCommand>
    {
        private readonly ICreditRepository _creditRepository;

        public SendCreditHandler(ICreditRepository creditRepository)
        {
            _creditRepository = creditRepository;
        }

        public async Task<bool> HandleAsync(CreditValueCommand message, CancellationToken cancellationToken = default)
        {
            var credit = new CreditEntity
            {
                Id = Guid.NewGuid(),
                Amount = message.Amount,
                DateToCredit = message.DateToCredit,
                OperationId = message.OperationId,
                Status = CreditStatus.Pending
            };

            await _creditRepository.SaveCredit(credit, cancellationToken);

            return true;
        }
    }
}
