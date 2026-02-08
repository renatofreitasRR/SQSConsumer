using ConsumerExample.Application.Services;
using ConsumerExample.Domain.Events;

namespace ConsumerExample.Application.Handlers
{
    public class EnviarEventoBloqueioRealizadoHandler : IDomainEventHandler<BloqueioRealizadoEvent>
    {
        private readonly ILoggerService<EnviarEventoBloqueioRealizadoHandler> _logger;
        public EnviarEventoBloqueioRealizadoHandler(ILoggerService<EnviarEventoBloqueioRealizadoHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(BloqueioRealizadoEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Evento de bloqueio realizado recebido. Enviando para o tópico...");
        }
    }
}
