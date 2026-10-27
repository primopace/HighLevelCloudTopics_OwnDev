using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedContracts.Topologies;
using System.Text;
using System.Text.Json;

namespace MessagingConsumer.Messaging
{
    public class RabbitMqBusBootstrap : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMqBusBootstrap> _logger;

        public RabbitMqBusBootstrap(IServiceProvider serviceProvider, ILogger<RabbitMqBusBootstrap> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync(stoppingToken);
            using var channel = await connection.CreateChannelAsync();

            // 1. Setup topologies (Exchanges)
            await channel.ExchangeDeclareAsync(PaymentTopology.PaymentExchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);

            // 2. Configure the Payment Failure Queue
            string compensationQueue = "order-payment-failure-queue";
            await channel.QueueDeclareAsync(compensationQueue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await channel.QueueBindAsync(compensationQueue, PaymentTopology.PaymentExchange, PaymentTopology.PaymentFailedKey, cancellationToken: stoppingToken);

            // 3. Register the Consumer Listener Pipeline
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var jsonMessage = Encoding.UTF8.GetString(body);
                var messageData = JsonSerializer.Deserialize<PaymentFailedEvent>(jsonMessage);

                // Fetch the consumer using Scoped DI inside the network background thread
                using (var scope = _serviceProvider.CreateScope())
                {
                    var concreteConsumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer<PaymentFailedEvent>>();

                    try
                    {
                        // Pass tracking data directly to our clean isolated class
                        await concreteConsumer.ConsumeAsync(messageData!, ea.DeliveryTag, ea.RoutingKey);

                        // Acknowledge upon zero exceptions thrown
                        await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to execute consumer fallback logic.");
                        await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                    }
                }
            };

            // Turn on the network switch
            await channel.BasicConsumeAsync(compensationQueue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            _logger.LogInformation("RabbitMQ Consumer Bus Engine is listening safely...");

            // Wait gracefully until the microservice application shuts down
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
