using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Order.API.RedisCache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);

            if (data == null)
                return default;

            return JsonSerializer.Deserialize<T>(data);
        }
        //await cache.SetStringAsync("Order_123", "data");
        //instance will save like OrderService:Order_123
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    expiry ?? TimeSpan.FromMinutes(5)
            };

            var json = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(key, json, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
