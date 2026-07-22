namespace Exam.API.Domain
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IDomainEvent domainEvent);
    }
}

