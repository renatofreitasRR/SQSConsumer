using SQSConsumerWorker.Domain;
using SQSConsumerWorker.Handlers;

namespace SQSConsumerWorker.UseCases
{
    public class MovimentoLiquidacaoEntradaHandler : IMessageHandler<MovimentoLiquidacaoEntradaEvent>
    {
        public Task<bool> HandleAsync(MovimentoLiquidacaoEntradaEvent message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
