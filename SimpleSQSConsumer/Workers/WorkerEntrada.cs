using SimpleSQSConsumer.Domain;
using SimpleSQSConsumer.Handlers;
using SimpleSQSConsumer.Services;

namespace SimpleSQSConsumer
{
    public class WorkerEntrada : BackgroundService
    {
        private readonly IQueueService _queueService;
        private readonly IHandler<MovimentosEntradaMessage> _handler;
        private readonly string _queueUrl = "";

        public WorkerEntrada(IQueueService queueService, IHandler<MovimentosEntradaMessage> handler)
        {
            _queueService = queueService;
            _handler = handler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _queueService.ConsumeQueueAsync(_queueUrl, _handler, stoppingToken);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao iniciar o WorkerEntrada: {ex.Message}");
                throw;
            }
        }
    }
}
