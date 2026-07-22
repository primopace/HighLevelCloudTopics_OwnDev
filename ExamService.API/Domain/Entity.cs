using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.API.Domain
{
    public  abstract class Entity
    {
        public Guid Id { get; protected set; }
        [NotMapped]
        private readonly List<IDomainEvent> _domainEvents = new();
        [NotMapped]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;
        protected void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents.Add(eventItem);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}

//BaseEntity=Entity
//     │
//     ▼
//AggregateRoot (ExamModel)
//     │
//     ▼
//ValueObjects / Child Entities