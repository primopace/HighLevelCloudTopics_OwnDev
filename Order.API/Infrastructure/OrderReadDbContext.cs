using Microsoft.EntityFrameworkCore;
using Order.API.Entities;

namespace Order.API.Infrastructure
{
    public class OrderReadDbContext : DbContext
    {
        public OrderReadDbContext(DbContextOptions<OrderReadDbContext> options)
            : base(options)
        {
        }

        public DbSet<OrderView> OrderViews { get; set; }
    }
}
