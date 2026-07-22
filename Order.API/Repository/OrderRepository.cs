using Microsoft.EntityFrameworkCore;
using Order.API.Infrastructure;

namespace Order.API.Repository
{
    public class OrderRepository :
      GenericRepository<Order.API.Entities.Order>,
      IOrderRepository
    {
        public OrderRepository(OrderWriteDbContext context)
            : base(context)
        {
        }

        public async Task<Order.API.Entities.Order?> GetOrderWithItems(Guid orderId)
        {
            return await _context.Orders
                //.Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<bool> Exists(Guid orderId)
        {
            return await _context.Orders
                .AnyAsync(x => x.Id == orderId);
        }
    }
}
