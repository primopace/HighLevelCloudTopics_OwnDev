using Order.API.Entities;

namespace Order.API.Repository
{
    public interface IOrderRepository
    {
        Task<Order.API.Entities.Order?> GetOrderWithItems(Guid orderId);

        Task<bool> Exists(Guid orderId);
    }
}
