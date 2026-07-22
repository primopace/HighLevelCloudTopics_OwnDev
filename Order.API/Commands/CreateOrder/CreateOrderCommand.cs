using Order.API.Mediator;

namespace Order.API.OrderCommands.Command
{
    public class CreateOrderCommand : ICommand<Guid>
    {
        public Guid CustomerId { get; }
        public Guid ProductId { get; }
        public int Quantity { get; }
        public decimal Amount { get; set; }

        public CreateOrderCommand(Guid customerId, Guid productId, int quantity, decimal amount)
        {
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
            Amount = amount;
        }
    }
}
