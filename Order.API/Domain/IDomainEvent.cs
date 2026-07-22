namespace Order.API.Domain
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
