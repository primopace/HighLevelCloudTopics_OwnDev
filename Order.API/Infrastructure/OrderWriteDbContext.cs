using Microsoft.EntityFrameworkCore;
using Order.API.Domain;
using Order.API.Entities;

namespace Order.API.Infrastructure
{
    public class OrderWriteDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _eventDispatcher;

        public OrderWriteDbContext(
            DbContextOptions<OrderWriteDbContext> options,
            IDomainEventDispatcher eventDispatcher)
            : base(options)
        {
            _eventDispatcher = eventDispatcher;
        }

        public DbSet<Order.API.Entities.Order> Orders { get; set; }

        public DbSet<OrderEventStore> OrderEvents { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _eventDispatcher.DispatchAsync(domainEvent);
            }

            domainEntities.ForEach(entity =>
                entity.Entity.ClearDomainEvents());

            return result;
        }
    }
}
//Add-Migration OrderWriteMig_2 -Context OrderWriteDbContext -OutputDir Migrations/Write
//Update-Database -Context OrderWriteDbContext