using ConsumerExample.Domain.Models;
using ConsumerExample.Worker.Services;
using ConsumerExample.Worker.UseCases;

namespace ConsumerExample.Worker
{
    public class WorkerBloqueio : BackgroundService
    {
        private readonly ILogger<WorkerBloqueio> _logger;
        private readonly IQueueConsumerService<ProcessarBloqueioUseCase, SolicitacaoBloqueioRequest> _queueConsumer;

        public WorkerBloqueio(ILogger<WorkerBloqueio> logger, IQueueConsumerService<ProcessarBloqueioUseCase, SolicitacaoBloqueioRequest> queueConsumer)
        {
            _logger = logger;
            _queueConsumer = queueConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("WorkerBloqueio iniciado em: {time}", DateTimeOffset.Now);

            try
            {
                await _queueConsumer.StartConsumingAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Um erro crítico ocorreu no WorkerBloqueio: {message}", ex.Message);

                await StopAsync(stoppingToken);

                Dispose();

            }
        }
    }
}
