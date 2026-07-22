using BuildingBlocks.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Infrastructure;

namespace Order.API.Consumers
{
    public class PaymentFailedConsumer:IConsumer<PaymentFailedEvent>
    {
        private readonly OrderWriteDbContext _orderDbContext;
        public PaymentFailedConsumer(OrderWriteDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _orderDbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
            //order.Status = "Cancelled";
            await _orderDbContext.SaveChangesAsync();
        }

    }
}
