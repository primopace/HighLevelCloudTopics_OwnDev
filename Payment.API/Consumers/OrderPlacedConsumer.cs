using BuildingBlocks.Events;
using MassTransit;
using Payment.API.Domain;
using Payment.API.Infrastructure;


namespace Payment.API.Consumers
{
    //without saga
    public class OrderPlacedConsumer
    { 
    //public class OrderPlacedConsumer:IConsumer<OrderPlacedEvent>
    //{
    //    private readonly PaymentDbContext _paymentDbContext;
    //    private readonly IPublishEndpoint _publishEndpoint;
    //    public OrderPlacedConsumer(PaymentDbContext paymentDbContext, IPublishEndpoint publishEndpoint)
    //    {
    //        _paymentDbContext = paymentDbContext;
    //        _publishEndpoint = publishEndpoint;
    //    }

    //    public async Task Consume(ConsumeContext<OrderPlacedEvent> context) 
    //    {
    //        var payment = new PaymentEntity
    //        {
    //            Id = Guid.NewGuid(),
    //            OrderId = context.Message.OrderId,
    //            Amount =context.Message.Amount,
    //            Status = "Completed"
    //        };
    //        _paymentDbContext.Payments.Add(payment);
    //        await _paymentDbContext.SaveChangesAsync();

    //        await _publishEndpoint.Publish(new PaymentCompletedEvent(payment.OrderId));
    //        //or
    //        await _publishEndpoint.Publish(
    //new PaymentFailedEvent(payment.OrderId));


    //    }


   }
}
