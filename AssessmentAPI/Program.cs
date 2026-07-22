using Assessment.API.Hubs;
using Assessment.API.Services;
using Assessment.Application.Services;
using AssessmentAPI.Middleware;
using AssessmentAPI.Requirements;
using Exam.Contracts.Protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7021";
        options.RequireHttpsMetadata = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = "assessment_api",

            RoleClaimType = "role",
            NameClaimType = "name"
        };
    });
builder.Services.AddScoped<ExamEvaluationService>();

builder.Services.AddSingleton<IAuthorizationHandler, ExamWindowHandler>();
builder.Services.AddSingleton<ExaminationService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin.Access", policy =>
        policy.RequireClaim("role", "Admin"));

    options.AddPolicy("Assessment.Read", policy =>
        policy.RequireClaim("scope", "assessment.read"));

    options.AddPolicy("Assessment.Write", policy =>
        policy.RequireClaim("scope", "assessment.write"));

    options.AddPolicy("Exam.Start", policy =>
        policy.RequireClaim("scope", "exam.start"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Exam.Window",
        policy => policy.AddRequirements(new ExamWindowRequirement()));
});

builder.Services.AddSignalR();

builder.Services.AddScoped<RealtimeNotificationService>();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Proctoring System",
        Description = "Plug and play Assessments Test",


    });
});


builder.Services.AddGrpcClient<ExamGrpc.ExamGrpcClient>(o =>
{
    o.Address = new Uri("https://localhost:7248");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy
            .WithOrigins("https://localhost:7065")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();
app.MapHub<AssessmentNotificationHub>("/hubs/assessment");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("BlazorClient");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

app.Use(async (context, next) =>
{
    await next();

    if (context.User.Identity?.IsAuthenticated == true)
    {
        // Persist audit log:
        // UserId, Action, Timestamp, IP, Resource
    }
});
app.MapControllers();

app.Run();
