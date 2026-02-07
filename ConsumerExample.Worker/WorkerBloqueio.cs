using ConsumerExample.Domain.Models;
using ConsumerExample.Worker.Services;
using ConsumerExample.Worker.UseCases;

namespace ConsumerExample.Worker
{
    public class WorkerBloqueio : BackgroundService
    {
        private readonly ILogger<WorkerBloqueio> _logger;
        private readonly IQueueConsumerService<ProcessarBloqueioUseCase, SolicitaoBloqueioRequest> _queueConsumer;

        public WorkerBloqueio(ILogger<WorkerBloqueio> logger, IQueueConsumerService<ProcessarBloqueioUseCase, SolicitaoBloqueioRequest> queueConsumer)
        {
            _logger = logger;
            _queueConsumer = queueConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WorkerBloqueio starting at: {time}", DateTimeOffset.Now);

            try
            {
                await _queueConsumer.StartConsumingAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                await StopAsync(stoppingToken);

                Dispose();

                _logger.LogError(ex, "An error occurred in WorkerBloqueio: {message}", ex.Message);
            }
        }
    }
}
