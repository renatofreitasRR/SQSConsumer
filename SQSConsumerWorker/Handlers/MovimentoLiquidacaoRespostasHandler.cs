using SQSConsumerWorker.Domain;
using SQSConsumerWorker.Handlers;

namespace SQSConsumerWorker.UseCases
{
    public class MovimentoLiquidacaoRespostasHandler : IMessageHandler<MovimentoLiquidacaoRespostaEvent>
    {
        public Task<bool> HandleAsync(MovimentoLiquidacaoRespostaEvent message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
