
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderSaga.API.Saga;

namespace OrderSaga.API.Infrastructure
{
    //public class SagaDbContext : DbContext
    //{
    //    public SagaDbContext(DbContextOptions<SagaDbContext> options)
    //    : base(options)
    //    {
    //    }

    //    public DbSet<OrderSagaState> OrderSagaStates { get; set; }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<OrderSagaState>()
    //            .HasKey(x => x.CorrelationId);
    //    }
    //}

    public class SagaDbContext : MassTransit.EntityFrameworkCoreIntegration.SagaDbContext
    {
        public SagaDbContext(DbContextOptions options)
            : base(options)
        {
        }
     
        protected override IEnumerable<ISagaClassMap>
            Configurations
        {
            get { yield return new OrderSagaStateMap(); }
        }
    }

    public class OrderSagaStateMap :
    SagaClassMap<OrderSagaState>
    {
        protected override void Configure(
            EntityTypeBuilder<OrderSagaState> entity,
            ModelBuilder model)
        {
            entity.Property(x => x.CurrentState);
        }
    }

}
