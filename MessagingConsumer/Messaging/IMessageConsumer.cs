namespace MessagingConsumer.Messaging
{
    public interface IMessageConsumer<T> where T : class
    {
        Task ConsumeAsync(T message, ulong deliveryTag, string routingKey);
    }
}
