using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderSaga.API.Infrastructure;
using OrderSaga.API.Saga;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddDbContext<SagaDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderSagaStateMachine, OrderSagaState>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;

            r.AddDbContext<DbContext, SagaDbContext>((provider, builder) =>
            {
                builder.UseSqlServer(
                    provider.GetRequiredService<IConfiguration>()
                    .GetConnectionString("DefaultConnection"));
            });
        });

    x.UsingRabbitMq((context, cfg) =>
    {
        //cfg.Host("rabbitmq");

        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

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
