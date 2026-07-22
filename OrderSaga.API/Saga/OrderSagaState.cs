using MassTransit;

namespace OrderSaga.API.Saga
{
    public class OrderSagaState: SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }

        public Guid OrderId { get; set; }

        public bool PaymentCompleted { get; set; }

        public bool InventoryReserved { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
