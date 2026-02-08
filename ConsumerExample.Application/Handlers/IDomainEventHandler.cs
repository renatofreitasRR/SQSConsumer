using ConsumerExample.Domain.Events;

namespace ConsumerExample.Application.Handlers
{
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        Task Handle(T domainEvent, CancellationToken cancellationToken = default);
    }
}
