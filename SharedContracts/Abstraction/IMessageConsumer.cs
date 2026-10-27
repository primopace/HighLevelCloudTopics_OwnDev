using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Abstraction
{
    /// <summary>
    /// A universal blueprint for any microservice consumer handling native AMQP events.
    /// </summary>
    /// <typeparam name="T">The immutable record message type being processed.</typeparam>
    public interface IMessageConsumer<in T> where T : class
    {
        /// <summary>
        /// Executes the business logic for a specific message pushed by RabbitMQ.
        /// </summary>
        /// <param name="message">The deserialized C# record payload.</param>
        /// <param name="deliveryTag">The unique network identifier assigned by the broker.</param>
        /// <param name="routingKey">The exact routing key used to publish this message.</param>
        Task ConsumeAsync(T message, ulong deliveryTag, string routingKey);
    }
}
