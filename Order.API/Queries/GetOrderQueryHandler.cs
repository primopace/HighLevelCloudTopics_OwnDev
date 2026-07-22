using Microsoft.EntityFrameworkCore;
using Order.API.Entities;
using Order.API.Infrastructure;
using Order.API.Mediator;

namespace Order.API.Queries
{
    public class GetOrderQueryHandler
    : IQueryHandler<GetOrderQuery, OrderView>
    {
        private readonly OrderReadDbContext _context;

        public GetOrderQueryHandler(OrderReadDbContext context)
        {
            _context = context;
        }

        public async Task<OrderView> Handle(GetOrderQuery query)
        {
            var order = await _context.OrderViews
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderId == query.Id);

            if (order == null)
                throw new Exception("Order not found");

            return order;
        }
    }
}
