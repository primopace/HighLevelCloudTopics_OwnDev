namespace Exam.API.Domain
{
    public abstract class DomainEvent: IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public string? CorrelationId { get; set; }//concrete member
        public Guid EventId { get; } = Guid.NewGuid();//concrete member
    }
}
