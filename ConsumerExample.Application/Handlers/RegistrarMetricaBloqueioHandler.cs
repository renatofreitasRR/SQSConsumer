using ConsumerExample.Application.Services;
using ConsumerExample.Domain.Events;

namespace ConsumerExample.Application.Handlers
{
    public class RegistrarMetricaBloqueioHandler : IDomainEventHandler<BloqueioRealizadoEvent>
    {
        private readonly ILoggerService<RegistrarMetricaBloqueioHandler> _logger;
        public RegistrarMetricaBloqueioHandler(ILoggerService<RegistrarMetricaBloqueioHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(BloqueioRealizadoEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Registrando metrica de bloqueio realizado...");
        }
    }
}
