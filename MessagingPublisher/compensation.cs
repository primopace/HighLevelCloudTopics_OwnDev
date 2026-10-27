using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedContracts.Topologies;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace MessagingPublisher
{


    //        consumer.ReceivedAsync += async(model, ea) =>
    //{
    //    var body = ea.Body.ToArray();
    //        var jsonMessage = Encoding.UTF8.GetString(body);
    //        var orderData = JsonSerializer.Deserialize<OrderCreatedEvent>(jsonMessage);

    //        // Simulate a business logic failure (e.g., credit card declined)
    //        bool isCreditCardValid = false; 

    //    if (!isCreditCardValid)
    //    {
    //        Console.WriteLine($"[Payment Service] Payment failed for Order {orderData.OrderId}. Publishing compensation event...");

    //        // 1. Construct the Reversal/Compensating message payload
    //        var compensationEvent = new PaymentFailedEvent(orderData.OrderId, "Card Declined / Insufficient Funds");
    //        var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(compensationEvent));

    //        var properties = new BasicProperties { DeliveryMode = DeliveryModes.Persistent };

    //        // 2. Publish the failure event natively out to the network
    //        await channel.BasicPublishAsync(
    //            exchange: PaymentTopology.PaymentExchange,
    //            routingKey: PaymentTopology.PaymentFailedKey,
    //            mandatory: true,
    //            basicProperties: properties,
    //            body: payload);

    //        // 3. Positively acknowledge the original incoming message. 
    //        // We caught the error safely and handled it via a Saga event, so this message is "done".
    //        await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
    //        return;
    //    }

    //    // Process successful payment logic here...
    //    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
    //    };
    //    }


}


//string orderCompensationQueueName = "order-payment-failure-queue";

//// Declare queue and bind to the payment exchange
//await channel.QueueDeclareAsync(orderCompensationQueueName, durable: true, exclusive: false, autoDelete: false);
//await channel.QueueBindAsync(
//    queue: orderCompensationQueueName,
//    exchange: PaymentTopology.PaymentExchange,
//    routingKey: PaymentTopology.PaymentFailedKey);

//var rollbackConsumer = new AsyncEventingBasicConsumer(channel);
//rollbackConsumer.ReceivedAsync += async (model, ea) =>
//{
//    var body = ea.Body.ToArray();
//    var jsonMessage = Encoding.UTF8.GetString(body);
//    var failureData = JsonSerializer.Deserialize<PaymentFailedEvent>(jsonMessage);

//    Console.WriteLine($"[Order Service] Compensating action triggered for Order: {failureData.OrderId}");

//    // NATIVE ROLLBACK / COMPENSATION WORK:
//    // Run an internal database command here: 
//    // "UPDATE Orders SET Status = 'Cancelled' WHERE Id = @OrderId"
//    await CancelOrderInDatabaseAsync(failureData.OrderId, failureData.Reason);

//    // Acknowledge the compensation message is handled
//    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
//};

//await channel.BasicConsumeAsync(orderCompensationQueueName, autoAck: false, consumer: rollbackConsumer);

