using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedContracts.Topologies;

namespace MessagingConsumer
{
    public class RabbitMqConsumerService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // The payment service defines its own unique queue mailbox name
            string myQueueName = "payment-service-queue";

            await channel.QueueDeclareAsync(myQueueName, durable: true, exclusive: false, autoDelete: false);

            // Bind my queue to the shared exchange name using the exact shared routing key contract
            await channel.QueueBindAsync(
                queue: myQueueName,
                exchange: RabbitMqTopology.ExchangeName,
                routingKey: RabbitMqTopology.RoutingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                Console.WriteLine($"Received shared event data frame matching key: {ea.RoutingKey}");
                await channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await channel.BasicConsumeAsync(myQueueName, autoAck: false, consumer: consumer);


            // INSTEAD OF TIMEOUT.INFINITE:
            // This keeps the service running efficiently until the .NET host 
            // tells the application to stop (e.g., during a deployment or Ctrl+C)
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
