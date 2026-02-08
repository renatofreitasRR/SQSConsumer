using ConsumerExample.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsumerExample.Domain.Entities
{
    public abstract class BaseEntity
    {
        [NotMapped]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        [NotMapped]
        private List<string> Errors { get; set; } = new List<string>();
        [NotMapped]
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public string GetErrorsAsString()
        {
            return string.Join("; ", Errors);
        }

        public bool HasErrors => Errors.Any();
    }
}
