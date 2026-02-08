using ConsumerExample.Domain.Entities;

namespace ConsumerExample.Domain.Events
{
    public class BloqueioRealizadoEvent : IDomainEvent
    {
        public Guid BloqueioId { get; }
        public string CodigoProtocolo { get; }
        public int OrdemBloqueio { get; }
        public DateTime DataMovimento { get; }
        public DateTime OccurredAt { get; } = DateTime.UtcNow;

        public BloqueioRealizadoEvent(BloqueioEntity bloqueio)
        {
            BloqueioId = bloqueio.Id;
            CodigoProtocolo = bloqueio.CodigoProtocolo;
            OrdemBloqueio = bloqueio.OrdemBloqueio;
            DataMovimento = bloqueio.DataMovimento;
        }
    }
}
