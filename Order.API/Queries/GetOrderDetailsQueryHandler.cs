using Dapper;
using Order.API.Entities;
using Order.API.Infrastructure;
using Order.API.Mediator;
using Order.API.RedisCache;

namespace Order.API.Queries
{
    public class GetOrderDetailsQueryHandler : IQueryHandler<GetOrderDetailsQuery, OrderView>
    {

        private readonly DapperContext _dapper;
        private readonly ICacheService _cache;

        public GetOrderDetailsQueryHandler(
            DapperContext dapper,
            ICacheService cache)
        {
            _dapper = dapper;
            _cache = cache;
        }

        public async Task<OrderView> Handle(GetOrderDetailsQuery query)
        {
            var cacheKey = CacheKeys.Order(query.OrderId);

            var cached = await _cache.GetAsync<OrderView>(cacheKey);

            if (cached != null)
                return cached;

            var sql = """
            SELECT OrderId, CustomerId, Amount, Status
            FROM OrderViews
            WHERE OrderId = @OrderId
        """;

            using var connection = _dapper.CreateConnection();

            var order = await connection.QueryFirstAsync<OrderView>(
                sql,
                new { query.OrderId });

            await _cache.SetAsync(cacheKey, order);

            return order;
        }

    }
}
