using BuildingBlocks.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Infrastructure;

namespace Order.API.Consumers
{
    //without saga direct service to service
    //with saga means commands will publish from saga but the event can be publish to other services if needed.currently now we are publishing at final stage of transaction
    public class PaymentCompletedConsumer
    { 
    //public class PaymentCompletedConsumer:IConsumer<PaymentCompletedEvent>
    //{
    //    private readonly OrderDbContext _orderDbContext;
    //    public PaymentCompletedConsumer(OrderDbContext orderDbContext)
    //    {
    //        _orderDbContext = orderDbContext;
    //    }
    //    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    //    {
    //        var order = await _orderDbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
    //        order.Status = "Paid";
    //        await _orderDbContext.SaveChangesAsync();
    //    }

}
}
