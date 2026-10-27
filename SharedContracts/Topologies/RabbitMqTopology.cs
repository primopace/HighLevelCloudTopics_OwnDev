using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Topologies
{
    // 1. The immutable message contract
    public record OrderCreatedEvent(Guid OrderId, decimal Amount);

    // 2. A centralized location for your exchange and routing names
    public static class RabbitMqTopology
    {
        public const string ExchangeName = "order-events-exchange";
        public const string RoutingKey = "order.created"; // Strict convention: [entity].[action]
    }
}
