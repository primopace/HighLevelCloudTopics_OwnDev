using Order.API.Entities;
using Order.API.Mediator;

namespace Order.API.Queries
{
    public class GetOrderDetailsQuery : IQuery<OrderView>
    {
        public Guid OrderId { get; }

        public GetOrderDetailsQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
