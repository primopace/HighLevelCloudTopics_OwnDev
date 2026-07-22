using BuildingBlocks.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Infrastructure;

namespace Order.API.Consumers
{
    public class InventoryFailedConsumer :
     IConsumer<InventoryFailedEvent>
    {
        private readonly OrderWriteDbContext _context;

        public InventoryFailedConsumer(OrderWriteDbContext context)
        {
            _context = context;
        }

        public async Task Consume(
            ConsumeContext<InventoryFailedEvent> context)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(x =>
                    x.Id == context.Message.OrderId);

            if (order == null) return;

           // order.Status = "Cancelled";

            await _context.SaveChangesAsync();
        }
    }
}
