using BuildingBlocks.Events;
using Inventory.API.Domain;
using Inventory.API.Infrastructure;
using MassTransit;

namespace Inventory.API.Consumers
{
    //without saga
    public class PaymentCompletedConsumer
    { 
    //public class PaymentCompletedConsumer:IConsumer<PaymentCompletedEvent>
    //{
    //    private readonly InventoryDbContext _context;
    //    private readonly IPublishEndpoint _publish;

    //    public PaymentCompletedConsumer(
    //        InventoryDbContext context,
    //        IPublishEndpoint publish)
    //    {
    //        _context = context;
    //        _publish = publish;
    //    }

    //    public async Task Consume(
    //        ConsumeContext<PaymentCompletedEvent> context)
    //    {
    //        var reservation = new InventoryReservation
    //        {
    //            Id = Guid.NewGuid(),
    //            OrderId = context.Message.OrderId,
    //            Status = "Reserved"
    //        };

    //        _context.Reservations.Add(reservation);
    //        await _context.SaveChangesAsync();

    //        //await _publish.Publish(
    //        //    new InventoryReservedEvent(reservation.OrderId));
    //    }
    }
}
