using SimpleSQSConsumer.Domain;

namespace SimpleSQSConsumer.Handlers
{
    public class MovimentosRespostaHandler : IHandler<MovimentosRespostaMessage>
    {
        public async Task HandleAsync(MovimentosRespostaMessage message, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"MovimentosRespostaHandler {message.Id}");
        }
    }
}
