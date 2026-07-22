
using Order.API.Infrastructure;
using Order.API.Mediator;
using Order.API.OrderCommands.Command;
using Order.API.Entities;

namespace Order.API.Commands.CreateOrder
{
    public class CreateOrderCommandHandler
     : ICommandHandler<CreateOrderCommand, Guid>
    {
        private readonly OrderWriteDbContext _context;

        public CreateOrderCommandHandler(OrderWriteDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateOrderCommand command)
        {
            var order = new Order.API.Entities.Order(command.CustomerId, command.Amount);

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order.Id;
        }
    }
}
