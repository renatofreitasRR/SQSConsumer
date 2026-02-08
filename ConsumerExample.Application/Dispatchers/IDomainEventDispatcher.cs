using ConsumerExample.Domain.Events;

namespace ConsumerExample.Application.Dispatchers
{
    public interface IDomainEventDispatcher
    {
        Task SendAsync<TEvent>(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default) where TEvent : IDomainEvent;
    }
}
