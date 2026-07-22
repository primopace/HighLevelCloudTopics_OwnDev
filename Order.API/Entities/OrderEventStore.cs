namespace Order.API.Entities
{
    public class OrderEventStore
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public string EventType { get; set; }

        public string EventData { get; set; }

        public DateTime OccurredOn { get; set; }
        public string Version { get; set; }
    }
}
