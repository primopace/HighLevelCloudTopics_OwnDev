using Contracts.Events;
using Exam.API.Data;
using Exam.API.Messaging;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Exam.API.PublisherService
{
    public class OutboxPublisher : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutboxPublisher(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<ExamDbContext>();
                var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                var messages = await db.OutboxMessages
                    .Where(x => x.ProcessedOn == null)
                    .ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    if (!EventTypeResolver.EventMap.TryGetValue(message.Type, out var eventType))
                    {
                        Console.WriteLine($"Unknown event type: {message.Type}");
                        continue;
                    }

                    var domainEvent = JsonSerializer.Deserialize(message.Content, eventType);
                    if (domainEvent == null)
                        continue;

                    await publishEndpoint.Publish(domainEvent, stoppingToken);

                    message.ProcessedOn = DateTime.UtcNow;
                }

                await db.SaveChangesAsync(stoppingToken);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
