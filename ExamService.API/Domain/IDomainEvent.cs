namespace Exam.API.Domain
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
