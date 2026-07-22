using BuildingBlocks.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Infrastructure;

namespace Order.API.Consumers
{
    public class InventoryReservedConsumer :
    IConsumer<InventoryReservedEvent>
    {
        private readonly OrderWriteDbContext _context;

        public InventoryReservedConsumer(OrderWriteDbContext context)
        {
            _context = context;
        }

        public async Task Consume(
            ConsumeContext<InventoryReservedEvent> context)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(x =>
                    x.Id == context.Message.OrderId);

            if (order == null) return;

          //  order.Status = "Completed";

            await _context.SaveChangesAsync();
        }
    }
}
