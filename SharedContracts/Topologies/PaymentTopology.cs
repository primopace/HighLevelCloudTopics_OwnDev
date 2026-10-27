using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Topologies
{
    public record PaymentFailedEvent(Guid OrderId, string Reason);

    public static class PaymentTopology
    {
        public const string PaymentExchange = "payment-events-topic-exchange";
        public const string PaymentFailedKey = "payment.status.failed";
    }
}
