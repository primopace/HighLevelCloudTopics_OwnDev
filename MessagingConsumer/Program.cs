using MessagingConsumer.Messaging;
using MessagingConsumer.Messaging.Consumers;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedContracts.Topologies;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();



// Register your isolated consumer class into DI
builder.Services.AddScoped<IMessageConsumer<PaymentFailedEvent>, PaymentFailedConsumer>();

// Spin up the background network topology framework runner
builder.Services.AddHostedService<RabbitMqBusBootstrap>();

///-----------------------------Directly consigured way
var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

string queueName = "payment-service-queue";

await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);

// Bind my queue to the shared exchange name using the exact shared routing key contract
await channel.QueueBindAsync(
    queue: queueName,
    exchange: RabbitMqTopology.ExchangeName,
    routingKey: RabbitMqTopology.RoutingKey);
// 1. Create the consumer tracking object linked to your network channel


var consumer = new AsyncEventingBasicConsumer(channel);

// 2. Attach an event handler. This code lies dormant until RabbitMQ pushes a message.
consumer.ReceivedAsync += async (model, eventArgs) =>
{
    // The message payload arrives here inside eventArgs
    byte[] body = eventArgs.Body.ToArray();
    string jsonMessage = Encoding.UTF8.GetString(body);

    // The DeliveryTag is a unique numeric ID RabbitMQ assigned to THIS specific delivery session
    ulong deliveryTag = eventArgs.DeliveryTag;

    Console.WriteLine($"[Payment Service] Alert! RabbitMQ just pushed a message: {jsonMessage}");

    try
    {
        // Execute Saga step (e.g., talk to a payment gateway API or save to database)
        //await ProcessPaymentSagaStepAsync(jsonMessage);

        // SUCCESS: Tell RabbitMQ it can safely erase this message from the disk/queue
        await channel.BasicAckAsync(
            deliveryTag: deliveryTag,
            multiple: false);

        Console.WriteLine($"[Payment Service] Message {deliveryTag} acknowledged successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Error] Processing failed. Re-queuing message: {ex.Message}");

        // FAILURE: Tell RabbitMQ the processing failed. 
        // requeue: true tells RabbitMQ to put it back at the front of the queue to try again.
        await channel.BasicNackAsync(
            deliveryTag: deliveryTag,
            multiple: false,
            requeue: true);
    }
};

// 3. Kickstart the continuous push listener. 
// autoAck: false forces RabbitMQ to wait for your Explicit BasicAckAsync call.
await channel.BasicConsumeAsync(
    queue: queueName,
    autoAck: false,
    consumer: consumer);

Console.WriteLine("Standing by. Waiting for RabbitMQ to push messages...");
// Keep the application running so the background thread can receive pushes
await Task.Delay(Timeout.Infinite);

//------------------------------


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
