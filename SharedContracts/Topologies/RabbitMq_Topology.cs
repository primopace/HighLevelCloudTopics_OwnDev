using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Topologies
{
    // 1. Immutable event records
    public record OrderCreated_Event(Guid OrderId, decimal Amount);
    public record OrderCancelled_Event(Guid OrderId, string Reason);

    // 2. The centralized topology registry
    public static class RabbitMq_Topology
    {
        public const string OrderExchange = "order-events-topic-exchange";

        // Publisher Keys (Publishers MUST use exact keys)
        public const string OrderCreatedKey = "order.lifecycle.created";
        public const string OrderCancelledKey = "order.lifecycle.cancelled";

        // Consumer Wildcard Binding Keys
        // Matches ONLY "order.lifecycle.created" or "order.lifecycle.cancelled"
        public const string SingleWordWildcardBinding = "order.lifecycle.*";

        // Matches "order.lifecycle.created", "order.lifecycle.cancelled", or deeper nested routing keys
        public const string MultiWordWildcardBinding = "order.#";
    }
}
