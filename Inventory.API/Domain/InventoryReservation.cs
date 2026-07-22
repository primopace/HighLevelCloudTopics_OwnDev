namespace Inventory.API.Domain
{
    public class InventoryReservation
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
