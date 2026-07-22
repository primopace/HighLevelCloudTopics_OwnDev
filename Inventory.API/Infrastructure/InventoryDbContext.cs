
using Inventory.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Infrastructure
{
    public class InventoryDbContext:DbContext
    {
        public DbSet<InventoryReservation> Reservations { get; set; }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {

        }
    }
}
