using Order.API.Domain;

namespace Order.API.Events
{
    public class OrderCompletedEvent : DomainEvent
    {
        public Guid OrderId { get; }

        public DateTime CompletedAt { get; }

        public OrderCompletedEvent(Guid orderId)
        {
            OrderId = orderId;
            CompletedAt = DateTime.UtcNow;
        }
    }
}
