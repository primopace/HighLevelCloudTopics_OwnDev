using BuildingBlocks.Events;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Domain;
using Order.API.Infrastructure;
using Order.API.OrderCommands.Command;
using Order.API.Queries;
using Order.API.Mediator;
using Order.API.Dto;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly IPublishEndpoint _publishEndpoint;
        public readonly OrderWriteDbContext _orderDbContext;
        public readonly IMediatr _mediator;
        public OrderController(OrderWriteDbContext orderDbContext, IPublishEndpoint publishEndpoint, IMediatr mediator)
        {
            _orderDbContext = orderDbContext;
            _publishEndpoint = publishEndpoint;
            _mediator = mediator;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult>CreateOrder(CreateOrderRequest request)
        {
            var order = new Order.API.Entities.Order(
               request.CustomerId,
             request.Amount);

            //var order = new Order.API.Entities.Order
            //{
            //    Id = Guid.NewGuid(),
            //    CustomerId = request.CustomerId,
            //    Amount = request.Amount,
            //    Status = "PendingPayment",
            //    CreatedAt =DateTime.UtcNow
            //};
            _orderDbContext.Add(order);
            await _orderDbContext.SaveChangesAsync();
            await _publishEndpoint.Publish(new OrderPlacedEvent(order.Id,order.CustomerId,order.Amount));
            return Ok(order);



        }

        [HttpPost("Create_Order")]
        public async Task<IActionResult> Create_Order(CreateOrderRequest request)
        {
            var command = new CreateOrderCommand(
                request.CustomerId,
                request.ProductId,
                request.Quantity,request.Amount);

            var orderId = await _mediator.Send(command);

            return Ok(orderId);
        }

        [HttpGet("GetOrder/{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var query = new GetOrderQuery(id);

            var order = await _mediator.Query(query);

            return Ok(order);
        }

        [HttpGet("GetOrderdetails/{id}")]
        public async Task<IActionResult> GetOrderdetails(Guid id)
        {
            var query = new GetOrderDetailsQuery(id);

            var order = await _mediator.Query(query);

            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder( Guid id,UpdateOrderRequest request)
        {
            var command = new UpdateOrderCommand(id, request.Quantity);

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok();
        }
    }


}
