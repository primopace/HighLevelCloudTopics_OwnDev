using Order.API.Domain;
using Order.API.Entities;
using Order.API.Events;
using Order.API.Infrastructure;
using RabbitMQ.Client;

namespace Order.API.Eventshandler
{
    public class OrderCreatedProjectionHandler:IDomainEventHandler<OrderCreatedEvent>      
    {
        private readonly OrderReadDbContext _readDb;
        public OrderCreatedProjectionHandler(OrderReadDbContext readDb)
        {
            _readDb = readDb;
        }

       public async Task Handle(OrderCreatedEvent notification)
        {
            var view = new OrderView
            {
                OrderId = notification.OrderId,
                CustomerId = notification.CustomerId,
                Amount = notification.Amount,
                Status = "Created",
                PaymentStatus = "Pending",
                InventoryStatus = "Pending",
                CreatedAt = DateTime.UtcNow

            };
             _readDb.OrderViews.Add(view);
            await _readDb.SaveChangesAsync();
        }
    }
}
