using Order.API.Entities;
using Order.API.Mediator;

namespace Order.API.Queries
{
    public class GetOrderQuery : IQuery<OrderView>
    {
        public Guid Id { get; }

        public GetOrderQuery(Guid orderId)
        {
            Id = orderId;
        }
    }
}
