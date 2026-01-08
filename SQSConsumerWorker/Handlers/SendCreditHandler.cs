using ComplexSQSConsumerWorker.Handlers.Contract;
using SQSConsumerWorker.Domain;

namespace SQSConsumerWorker.Handlers
{
    public class SendCreditHandler : IMessageHandler<CreditValueCommand>
    {
        public async Task<bool> HandleAsync(CreditValueCommand message, CancellationToken cancellationToken = default)
        {
            return true;
        }
    }
}
