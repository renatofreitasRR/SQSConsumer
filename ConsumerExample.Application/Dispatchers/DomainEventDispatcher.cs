using ConsumerExample.Application.Handlers;
using ConsumerExample.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace ConsumerExample.Application.Dispatchers
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
        private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider; 
        }

        public async Task SendAsync<TEvent>(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default) where TEvent : IDomainEvent
        {
            foreach (IDomainEvent domainEvent in domainEvents)
            {
                var handlers = _serviceProvider
                    .GetServices<IDomainEventHandler<TEvent>>();

                foreach (IDomainEventHandler<TEvent>? handler in handlers)
                {
                    if (handler is null) continue;

                    await handler.Handle((TEvent)domainEvent, cancellationToken);
                }
            }
        }
    }
}
