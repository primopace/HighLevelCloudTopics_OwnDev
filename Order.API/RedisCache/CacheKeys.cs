namespace Order.API.RedisCache
{
    public static class CacheKeys
    {
        public static string Order(Guid orderId)
            => $"order:{orderId}";

        public static string OrdersByCustomer(Guid customerId)
            => $"orders:customer:{customerId}";
    }
}
