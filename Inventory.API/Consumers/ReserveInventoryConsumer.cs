using BuildingBlocks.Events;
using Inventory.API.Domain;
using Inventory.API.Infrastructure;
using MassTransit;

namespace Inventory.API.Consumers
{
    public class ReserveInventoryConsumer :
     IConsumer<ReserveInventoryEvent>
    {
        private readonly InventoryDbContext _context;
        private readonly IPublishEndpoint _publish;

        public ReserveInventoryConsumer(
            InventoryDbContext context,
            IPublishEndpoint publish)
        {
            _context = context;
            _publish = publish;
        }

        public async Task Consume(
            ConsumeContext<ReserveInventoryEvent> context)
        {
            var message = context.Message;

            var reservation = new InventoryReservation
            {
                Id = Guid.NewGuid(),
                OrderId = message.OrderId,
                Status = "Processing",
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            bool inventoryAvailable = true;

            if (inventoryAvailable)
            {
                reservation.Status = "Reserved";

                await _publish.Publish(
                    new InventoryReservedEvent(message.OrderId));
            }
            else
            {
                reservation.Status = "Failed";

                await _publish.Publish(
                    new InventoryFailedEvent(
                        message.OrderId,
                        "Out of Stock"));
            }

            await _context.SaveChangesAsync();
        }
    }
}
