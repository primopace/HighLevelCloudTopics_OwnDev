namespace Order.API.Dto
{
    public class CreateOrderRequest
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; }
        public int Quantity { get; }
        public decimal Amount { get; set; }
    }
}
