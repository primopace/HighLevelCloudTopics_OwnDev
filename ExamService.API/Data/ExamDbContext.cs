using Exam.API.Application.Services;
using Exam.API.Domain;
using Exam.API.Outbox;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exam.API.Data
{
    public class ExamDbContext: DbContext, IUnitOfWork
    {
        private readonly IDomainEventDispatcher _dispatcher;
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<ExamModel> Exams => Set<ExamModel>();
        public ExamDbContext(DbContextOptions<ExamDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            var domainEntities =ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            foreach (var entity in domainEntities)
                entity.Entity.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
                await _dispatcher.DispatchAsync(domainEvent);

            return result;
        }



    }
}
