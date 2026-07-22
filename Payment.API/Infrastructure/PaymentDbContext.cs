using Microsoft.EntityFrameworkCore;
using Payment.API.Domain;
using Payment.API.Entities;

namespace Payment.API.Infrastructure
{
    public class PaymentDbContext:DbContext
    {
        public DbSet<PaymentEntity> Payments { get; set; }
        public DbSet<InboxMessage> InboxMessages { get; set; }
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options):base(options)
        {
             
        }
    }
}
