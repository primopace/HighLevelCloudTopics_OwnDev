using BuildingBlocks.Events;
using MassTransit;

namespace OrderSaga.API.Saga
{
    public class OrderSagaStateMachine : MassTransitStateMachine<OrderSagaState>
    {
        public State PaymentProcessing { get; private set; } = null!;
        public State InventoryProcessing { get; private set; } = null!;

        public Event<OrderPlacedEvent> OrderPlaced { get; private set; } = null!;

        public Event<PaymentCompletedEvent> PaymentCompleted { get; private set; } = null!;

        public Event<PaymentFailedEvent> PaymentFailed { get; private set; } = null!;

        public Event<InventoryReservedEvent> InventoryReserved { get; private set; } = null!;
        public Event<InventoryFailedEvent> InventoryFailed { get; private set; } = null!;

        public OrderSagaStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderPlaced,
                x => x.CorrelateById(m => m.Message.OrderId));

            Event(() => PaymentCompleted,
                x => x.CorrelateById(m => m.Message.OrderId));

            Event(() => PaymentFailed,
                x => x.CorrelateById(m => m.Message.OrderId));

            Event(() => InventoryReserved,
                x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => InventoryFailed,
           x => x.CorrelateById(m => m.Message.OrderId));

            Initially(
                When(OrderPlaced)
                    .Then(context =>
                    {
                        context.Saga.OrderId =
                            context.Message.OrderId;

                        context.Saga.CreatedAt =
                            DateTime.UtcNow;
                    })
                    .TransitionTo(PaymentProcessing)
                    .Publish(context =>
                        new PaymentRequestEvent(
                            context.Message.OrderId,
                            context.Message.Amount))
            );

            During(PaymentProcessing,

                When(PaymentCompleted)
                    .TransitionTo(InventoryProcessing)
                    .Publish(context =>
                        new ReserveInventoryEvent(
                            context.Message.OrderId)),

                When(PaymentFailed)
                    .Finalize()
            );

            During(InventoryProcessing,

                When(InventoryReserved)
        .Finalize(),

    When(InventoryFailed)
        .Finalize()
        );

            SetCompletedWhenFinalized();
        }
    }
}
