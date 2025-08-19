using SimpleSQSConsumer.Domain;

namespace SimpleSQSConsumer.Handlers
{
    public class MovimentosEntradaHandler : IHandler<MovimentosEntradaMessage>
    {
        public async Task HandleAsync(MovimentosEntradaMessage message, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"MovimentosEntradaHandler {message.Id}");
        }
    }
}
