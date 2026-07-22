using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Events
{
    public record OrderPlacedEvent(Guid OrderId,Guid CustomerId,decimal Amount);
    public record PaymentRequestEvent(Guid OrderId ,decimal Amount);
    public record PaymentCompletedEvent(Guid OrderId);
    public record PaymentFailedEvent(Guid OrderId,string message);
    public record ReserveInventoryEvent(Guid OrderId);
    public record InventoryReservedEvent(Guid OrderId);
    public record InventoryFailedEvent(Guid OrderId,string message);
    public record CacheInvalidationEvent
    (
        string CacheKey
    );

}
    