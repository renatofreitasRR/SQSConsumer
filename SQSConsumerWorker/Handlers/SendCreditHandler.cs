using SQSConsumerWorker.Domain;
using SQSConsumerWorker.Handlers;

namespace SQSConsumerWorker.UseCases
{
    public class SendCreditHandler : IMessageHandler<CreditValueCommand>
    {
        public Task<bool> HandleAsync(CreditValueCommand message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
