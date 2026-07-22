using BuildingBlocks.Events;
using MassTransit;
using Order.API.Domain;
using Order.API.Events;

namespace Order.API.Eventshandler
{
    public class OrderCreatedEventHandler
    : IDomainEventHandler<OrderCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(OrderCreatedEvent domainEvent)
        {
            await _publishEndpoint.Publish(
    new OrderPlacedEvent( domainEvent.OrderId,  domainEvent.CustomerId,domainEvent.Amount));
        }
    }
}
