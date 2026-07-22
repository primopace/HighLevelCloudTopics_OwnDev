using BuildingBlocks.Events;
using MassTransit;
using Order.API.Mediator;
using Order.API.OrderCommands.Command;
using Order.API.RedisCache;
using Order.API.Repository;

namespace Order.API.Commands.CreateOrder
{
    public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand, bool>
    {
        private readonly IOrderRepository _repo;
        private readonly ICacheService _cache;
        private readonly IPublishEndpoint _publish;
        public UpdateOrderCommandHandler(
          
            ICacheService cache, IOrderRepository repo, IPublishEndpoint publish)
        {
            
            _cache = cache;
            _repo = repo;
            _publish = publish;
        }

        public async Task<bool> Handle(UpdateOrderCommand command)
        {
            var order = await _repo.GetOrderWithItems(command.OrderId);

            if (order == null)
                return false;

            //order.UpdateQuantity(command.Quantity);
            // await _repo.SaveChangesAsync();

            // invalidate redis cache
            await _cache.RemoveAsync(CacheKeys.Order(order.Id));
            await _publish.Publish(
       new CacheInvalidationEvent($"order:{order.Id}")
   );
            return true;
        }
    }
}
