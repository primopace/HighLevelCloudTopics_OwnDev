using BuildingBlocks.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.API.Domain;
using Payment.API.Entities;
using Payment.API.Infrastructure;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace Payment.API.Consumers
{
    public class PaymentRequestConsumer :
        IConsumer<PaymentRequestEvent>
    {
        private readonly PaymentDbContext _context;
        private readonly IPublishEndpoint _publish;

        public PaymentRequestConsumer(
            PaymentDbContext context,
            IPublishEndpoint publish)
        {
            _context = context;
            _publish = publish;
        }

        public async Task Consume(
            ConsumeContext<PaymentRequestEvent> paymentContext)
        {
            var messageId = paymentContext.MessageId?? Guid.NewGuid();

            bool alreadyProcessed = await _context.InboxMessages
           .AnyAsync(x => x.MessageId == messageId);
            var payment = new PaymentEntity
            {
                Id = Guid.NewGuid(),
                OrderId = paymentContext.Message.OrderId,
                Amount = paymentContext.Message.Amount,
                Status = "Processing",
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);

            _context.InboxMessages.Add(new InboxMessage
            {
                Id = Guid.NewGuid(),
                MessageId = messageId,
                Consumer = nameof(PaymentRequestConsumer),
                ProcessedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            bool paymentSuccess = true;

            if (paymentSuccess)
            {
                payment.Status = "Completed";

                await _publish.Publish(
                    new PaymentCompletedEvent(paymentContext.Message.OrderId));
            }
            else
            {
                payment.Status = "Failed";

                await _publish.Publish(
                    new PaymentFailedEvent(
                        paymentContext.Message.OrderId,
                        "Card Declined"));
            }

            await _context.SaveChangesAsync();
        }
    }
}
