using Order.API.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Entities
{
    public abstract class BaseEntity
    {
        [NotMapped]
        private readonly List<DomainEvent> _domainEvents = new();
        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
