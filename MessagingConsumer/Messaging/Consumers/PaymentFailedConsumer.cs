using SharedContracts.Topologies;

namespace MessagingConsumer.Messaging.Consumers
{
    public class PaymentFailedConsumer : IMessageConsumer<PaymentFailedEvent>
    {
        private readonly ILogger<PaymentFailedConsumer> _logger;
        // Inject your database context or repository smoothly via DI
        // private readonly IOrderRepository _orderRepository; 

        public PaymentFailedConsumer(ILogger<PaymentFailedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task ConsumeAsync(PaymentFailedEvent message, ulong deliveryTag, string routingKey)
        {
            _logger.LogWarning("Compensating transaction triggered for Order: {OrderId}", message.OrderId);

            // 1. Execute your isolated database operation
            // await _orderRepository.UpdateStatusAsync(message.OrderId, "Cancelled");
            await Task.Delay(100); // Simulating work

            _logger.LogInformation("Order {OrderId} successfully rolled back via compensation event.", message.OrderId);
        }
    }
}
