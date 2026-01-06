using SimpleSQSConsumer.Domain;
using SimpleSQSConsumer.Handlers;
using SimpleSQSConsumer.Services;

namespace SimpleSQSConsumer
{
    public class WorkerReturn : BackgroundService
    {
        private readonly IQueueService _queueService;
        private readonly IHandler<QueueReturnMessage> _handler;
        private readonly string _queueUrl;

        public WorkerReturn(IQueueService queueService, IHandler<QueueReturnMessage> handler, IConfiguration configuration)
        {
            _queueService = queueService;
            _handler = handler;
            _queueUrl = configuration["AWS:SQS:QueueReturnUrl"] ?? throw new InvalidOperationException("QueueReturnUrl não configurada no appsettings.json");   
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
