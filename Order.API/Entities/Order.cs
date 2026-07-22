using BuildingBlocks.Events;
using Order.API.Events;

namespace Order.API.Entities
{

    //Order is Aggregate
    public class Order : BaseEntity
    {
        public Guid Id { get; private set; }

        public Guid CustomerId { get; private set; }

        public decimal Amount { get; private set; }

        public string Status { get; private set; }

        public DateTime CreatedAt { get; private set; }
       
        public Order(Guid customerId, decimal amount)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            Amount = amount;
            Status = "Created";
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new OrderCreatedEvent(Id,customerId,amount));
        }

        public void Complete()
        {
            Status = "Completed";

            AddDomainEvent(new OrderCompletedEvent(Id));
        }

      



    }
}
