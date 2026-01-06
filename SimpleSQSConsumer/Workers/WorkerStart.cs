using SimpleSQSConsumer.Domain;
using SimpleSQSConsumer.Handlers;
using SimpleSQSConsumer.Services;

namespace SimpleSQSConsumer
{
    public class WorkerStart : BackgroundService
    {
        private readonly IQueueService _queueService;
        private readonly IHandler<QueueStartMessage> _handler;
        private readonly string _queueUrl;

        public WorkerStart(IQueueService queueService, IHandler<QueueStartMessage> handler, IConfiguration configuration)
        {
            _queueService = queueService;
            _handler = handler;
            _queueUrl = configuration["AWS:SQS:QueueStartUrl"] ?? throw new InvalidOperationException("QueueReturnUrl não configurada no appsettings.json");
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
