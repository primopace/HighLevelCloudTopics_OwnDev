using RabbitMQ.Client;
using SharedContracts.Abstraction;
using System.Text;
using System.Text.Json;

namespace MessagingPublisher.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
    {
        private readonly IConnection _connection;
        private IChannel? _channel;
        private readonly SemaphoreSlim _channelLock = new(1, 1);

        // Accept an established RabbitMQ connection injected from Dependency Injection (DI)
        public RabbitMqPublisher(IConnection connection)
        {
            _connection = connection;
        }

        private async Task<IChannel> GetChannelAsync()
        {
            if (_channel is { IsOpen: true }) return _channel;

            await _channelLock.WaitAsync();
            try
            {
                if (_channel is { IsOpen: true }) return _channel;

                // V7+ APPROACH: Enable confirmations inside the creation options object!
                var options = new CreateChannelOptions(
                    publisherConfirmationsEnabled: true,
                    publisherConfirmationTrackingEnabled: true
                );

                // Pass the options directly to the creation factory
                _channel = await _connection.CreateChannelAsync(options);
            }
            finally
            {
                _channelLock.Release();
            }

            return _channel;
        }

        public async Task PublishAsync<T>(string exchange, string routingKey, T message) where T : class
        {
            var channel = await GetChannelAsync();

            // 1. Serialize the strongly-typed C# record into standard JSON text strings
            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            // 2. Set enterprise persistence properties
            var properties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent // Ensures the event survives broker restarts
            };

            // 3. Publish natively to the targeted RabbitMQ Exchange wire
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: body);

            // Optional log tracking step
            Console.WriteLine($"[Publisher] Safely emitted message with routing key: {routingKey}");
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }
    }
}
