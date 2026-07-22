namespace Order.API.Domain
{
    public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task Handle(TEvent domainEvent);

    }
}
