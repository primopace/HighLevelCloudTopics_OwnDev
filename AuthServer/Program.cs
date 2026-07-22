using AuthServer.Certificates;
using AuthServer.Config;
using AuthServer.Data;
using AuthServer.Data.Seed;
using AuthServer.Identity;
using AuthServer.Services;
using AuthServer.Validators;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Runtime.ConstrainedExecution;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddIdentityServer(options =>
//{
//    options.Events.RaiseErrorEvents = true;
//    options.Events.RaiseFailureEvents = true;
//})
//     .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
//    .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
//    .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
//    .AddInMemoryClients(IdentityServerConfig.Clients)
//    .AddDeveloperSigningCredential();
//.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
var identityDb = builder.Configuration.GetConnectionString("IdentityDb");
var configDb = builder.Configuration.GetConnectionString("IdentityConfigDb");
var operationalDb = builder.Configuration.GetConnectionString("IdentityOperationalDb");

// --------------------
// ASP.NET Identity
// --------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityDb));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// --------------------
// Google Auth
// --------------------
builder.Services
    .AddAuthentication()
    .AddGoogle("Google", options =>
    {
        options.SignInScheme =
            IdentityServerConstants.ExternalCookieAuthenticationScheme;

        options.ClientId =
            builder.Configuration["Authentication:Google:ClientId"]!;

        options.ClientSecret =
            builder.Configuration["Authentication:Google:ClientSecret"]!;
    });



// --------------------
// IdentityServer
// --------------------
builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
.AddAspNetIdentity<ApplicationUser>()

// Clients, Scopes, Resources
.AddConfigurationStore(options =>
{
    options.ConfigureDbContext = b =>
        b.UseSqlServer(configDb,
            sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));
})

// Tokens, Codes, Refresh Tokens
.AddOperationalStore(options =>
{
    options.ConfigureDbContext = b =>
        b.UseSqlServer(operationalDb,
            sql => sql.MigrationsAssembly(typeof(Program).Assembly.FullName));

    options.EnableTokenCleanup = true;
    options.TokenCleanupInterval = 3600;
});
//for azure key vault certificate
if (builder.Environment.IsProduction())
{
    var keyVaultUrl = builder.Configuration["KeyVault:Url"];
    var certName = builder.Configuration["KeyVault:CertificateName"];

    var cert = KeyVaultCertificateLoader.LoadCertificate(
        keyVaultUrl!,
        certName!);
    //foreach (var cert in certs)
    //{
    builder.Services
    .AddIdentityServer()
    .AddSigningCredential(cert);

    //}
}
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
});

builder.Services.AddScoped<IProfileService, CustomProfileService>();
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy
            .WithOrigins("https://localhost:7065") // Blazor WASM
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("BlazorClient");
//app.UseRouting();
app.UseAuthentication();
app.UseIdentityServer();
app.UseHttpsRedirection();

app.UseAuthorization();
IdentityServerSeeder.Seed(app);
await AspNetIdentitySeeder.SeedAsync(app);
app.MapControllers();

app.Run();
