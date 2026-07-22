using Exam.Contracts.Protos;
using MassTransit;
using Result.API.Consumers;
using Result.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenLocalhost(7170, listenOptions =>
//    {
//        listenOptions.Protocols =
//            Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
//        listenOptions.UseHttps();
//    });
//});

//local
builder.Services.AddGrpcClient<ExamGrpc.ExamGrpcClient>(o =>
{
    o.Address = new Uri("https://localhost:7248");
});

//docker

//builder.Services.AddGrpcClient<ExamGrpc.ExamGrpcClient>(o =>
//{
//    o.Address = new Uri("http://localhost:8080");
//});

//docker

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(8080, o =>
//    {
//        o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
//    });
//});


//masstransit also asking mit liscence

//Docker
//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<ExamEvaluatedConsumer>();
//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("rabbitmq", "/", h =>
//        {
//            h.Username("guest");
//            h.Password("guest");
//        });
//        cfg.ConfigureEndpoints(context);
//    });
//});

//local
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ExamEvaluatedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
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
app.MapGrpcService<ResultGrpcService>();
app.Run();
