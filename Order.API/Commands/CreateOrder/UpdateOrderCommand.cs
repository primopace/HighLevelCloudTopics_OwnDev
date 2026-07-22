using Order.API.Mediator;

namespace Order.API.OrderCommands.Command
{
    public class UpdateOrderCommand : ICommand<bool>
    {
        public Guid OrderId { get; }

        public int Quantity { get; }

        public UpdateOrderCommand(Guid orderId, int quantity)
        {
            OrderId = orderId;
            Quantity = quantity;
        }
    }
}
