using MessagingPublisher.Messaging;
using RabbitMQ.Client;
using SharedContracts.Abstraction;
using SharedContracts.Topologies;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();




// 1. Register a Single Thread-Safe Native Connection to the Cluster
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory() { HostName = "localhost" };
    // Synchronously or asynchronously resolve the startup handshake
    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});

// 2. Register our custom structured publishing implementation
builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();





//var factory = new ConnectionFactory() { HostName = "localhost" };
//using var connection = await factory.CreateConnectionAsync();
//using var channel = await connection.CreateChannelAsync();

//// Declare the exchange safely
//await channel.ExchangeDeclareAsync(RabbitMqTopology.ExchangeName, ExchangeType.Topic, durable: true);

//// Create the record message
//var message = new OrderCreatedEvent(Guid.NewGuid(), 99.99m);
//var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

//var properties = new BasicProperties { DeliveryMode = DeliveryModes.Persistent };

//// Publish to the exchange using the explicit shared routing key
//await channel.BasicPublishAsync(
//    exchange: RabbitMqTopology.ExchangeName,
//    routingKey: RabbitMqTopology.RoutingKey,
//    mandatory: true,
//    basicProperties: properties,
//    body: body);



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
