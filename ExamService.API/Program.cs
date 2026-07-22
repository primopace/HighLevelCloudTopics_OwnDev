using Exam.API.Application.Services;
using Exam.API.Data;
using Exam.API.Domain;
using Exam.API.Domain.Handlers;
using Exam.API.Domain.Handlers.Implementation;
using Exam.API.Domain.Handlers.Implementation.Direct_DomainEvt_IntegEvt;
using Exam.API.PublisherService;
using Exam.API.Services;
using Exam.Contracts.Protos;

using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//local
builder.Services.AddGrpcClient<ResultGrpc.ResultGrpcClient>(o =>
{
    o.Address = new Uri("https://localhost:7170");
});
builder.Services.AddHostedService<OutboxPublisher>();
//docker

//builder.Services.AddGrpcClient<ResultGrpc.ResultGrpcClient>(o =>
//{
//    o.Address = new Uri("http://result-api:8080");
//});
//not mandate for 10.0
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenLocalhost(7248, listenOptions =>
//    {
//        listenOptions.Protocols =
//            Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
//        listenOptions.UseHttps();
//    });
//});

//docker
//builder.Services.AddMassTransit(x =>
//{
//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("rabbitmq", "/", h =>
//        {
//            h.Username("guest");
//            h.Password("guest");
//        });

//    });
//}
//);

//local
builder.Services.AddDbContext<ExamDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

    });
}
);

builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped<IExamService, Exam_Service>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();


builder.Services.AddScoped<IUnitOfWork>(sp =>
    sp.GetRequiredService<ExamDbContext>());

builder.Services.AddScoped<
    IDomainEventHandler<ExamEvaluatedDomainEvent>,
    PublishExamEvaluatedIntegrationEventHandler>();

builder.Services.AddScoped<
    IDomainEventHandler<ExamEvaluatedDomainEvent>,
    SaveExamResultHandler>();

builder.Services.AddScoped<
    IDomainEventHandler<ExamEvaluatedDomainEvent>,
    AuditExamEvaluatedHandler>();
builder.Services.AddScoped<
    IDomainEventHandler<ExamEvaluatedDomainEvent>,
    NotifyStudentExamEvaluatedHandler>();
//Repo to Unitofwork flow gothroght it

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
//Grpc.aspnetcore.server.reflection.just for testing
//if (app.Environment.IsDevelopment())
//{
//    app.MapGrpcReflectionService();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<ExamGrpcService>();

app.Run();
