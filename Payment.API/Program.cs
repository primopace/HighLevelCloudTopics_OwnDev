using MassTransit;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;
using Payment.API.Consumers;
using Payment.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<PaymentDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));









builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    //x.AddConsumer<OrderPlacedConsumer>();
    x.AddConsumer<PaymentRequestConsumer>();
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
        cfg.UseMessageRetry(r =>
        {
            r.Interval(3, TimeSpan.FromSeconds(5));
        });
        // Automatically create queues for consumers
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");

    options.InstanceName = "PaymentService:";
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
