using Order.API.Domain;

namespace Order.API.Events
{
    public class OrderCreatedEvent : DomainEvent
    {
        public Guid OrderId { get; }

        public Guid CustomerId { get; }

        public decimal Amount { get; }

        public OrderCreatedEvent(Guid orderId, Guid customerId, decimal amount)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Amount = amount;
        }


    }



}
