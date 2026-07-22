using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Commands.CreateOrder;
using Order.API.Consumers;
using Order.API.Domain;
using Order.API.Entities;
using Order.API.Events;
using Order.API.Eventshandler;
using Order.API.Infrastructure;
using Order.API.Mediator;
using Order.API.OrderCommands.Command;
using Order.API.Queries;
using Order.API.RedisCache;
using Order.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<OrderWriteDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("WriteDb")));

builder.Services.AddDbContext<OrderReadDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReadDb")));

//mediator
builder.Services.AddScoped<IMediatr, Mediatr>();
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped<
    ICommandHandler<CreateOrderCommand, Guid>,
    CreateOrderCommandHandler>();

builder.Services.AddScoped<
    IQueryHandler<GetOrderQuery, OrderView>,
    GetOrderQueryHandler>();

builder.Services.AddScoped<
    IQueryHandler<GetOrderDetailsQuery, OrderView>,
    GetOrderDetailsQueryHandler>();


//builder.Services.AddScoped<
//    IDomainEventHandler<OrderCreatedEvent>,
//    OrderCreatedEventHandler>();

builder.Services.AddScoped<
    IDomainEventHandler<OrderCreatedEvent>,
    OrderCreatedProjectionHandler>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "OrderService:";
});


builder.Services.AddScoped<DapperContext>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<InventoryFailedConsumer>();
    x.AddConsumer<InventoryReservedConsumer>();
    x.AddConsumer<PaymentFailedConsumer>();



    //or


    // Register all consumers in this service
    //x.AddConsumers(typeof(Program).Assembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        // Automatically create queues for consumers
        cfg.ConfigureEndpoints(context);
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
