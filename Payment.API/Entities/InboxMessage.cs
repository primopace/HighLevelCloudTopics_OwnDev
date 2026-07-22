namespace Payment.API.Entities
{
    public class InboxMessage
    {
        public Guid Id { get; set; }

        public Guid MessageId { get; set; }

        public string Consumer { get; set; }

        public DateTime ProcessedAt { get; set; }
    }
}
