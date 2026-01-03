using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using MyApi.Application.Users.CreateUser;
using MyApi.Application.Users.FindUser;
using MyApi.Application.Users.GetUser;
using MyApi.Endpoints;
using MyApi.Middleware;
using MyApi.Shared.Extensions;
using MySql.Data.MySqlClient;
using Serilog;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//
// Configuration
//
builder.Configuration.AddEnvironmentVariables();
var isDevelopment = builder.Environment.IsDevelopment();

if (isDevelopment)
{
    builder.Configuration.AddDotNetEnv(".env", LoadOptions.TraversePath());
}

// Default language
var culture = new CultureInfo("en-US");
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

// Enable global JsonPropertyName mapping for FluentValidation
builder.Services.AddFluentValidationJsonPropertyNames();

//
// Add services to the DI container
//
// Lifetimes
//
// Transient: Creates a new instance of the service every time you request it.
//
// Scoped: Creates a new instance for every scope. Each request is a Scope.
// Within the scope, it reuses the existing instance.
//
// Singleton: Creates a new Service only once during the application lifetime,
// and uses it everywhere.

// Middleware
builder.Services.AddScoped<ExceptionHandlerMiddleware>();
builder.Services.AddScoped<ValidationExceptionMiddleware>();
builder.Services.AddScoped<LocalizationMiddleware>();

// Database
var dsn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddScoped(provider =>
{
    var connection = new MySqlConnection(dsn);
    connection.Open();

    return connection;
});

builder.Services.AddTransient(provider =>
{
    var connection = provider.GetRequiredService<MySqlConnection>();

    return new QueryFactory(connection, new MySqlCompiler());
});

builder.Services.AddScoped<ITransaction, Transaction>();

// Localization
builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, MoStringLocalizerFactory>();

// Logging
builder.Logging.ClearProviders();

builder.Services.Add(ServiceDescriptor.Transient(typeof(ILogger<>), typeof(Logger<>)));
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();

// Serilog configuration
builder.Services.AddLogging(logging =>
{
    logging.AddSerilog(new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger());
});

// The MVC controllers using the Transient lifetime
// builder.Services.AddControllers().AddControllersAsServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Explore the API via Swagger UI http://localhost:<port>/swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application Services
builder.Services.AddScoped<UserFinder>();
builder.Services.AddScoped<UserFinderRepository>();
builder.Services.AddScoped<UserReader>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserCreator>();
builder.Services.AddScoped<UserCreatorRepository>();
builder.Services.AddScoped<UserCreatorValidator>();

var app = builder.Build();

//
// Middleware
//
app.UseExceptionHandlerMiddleware();
app.UseValidationExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable request localization in order to determine
// the users desired language based on the Accept-Language header.
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
});
app.UseMiddleware<LocalizationMiddleware>();

// app.UseHttpsRedirection();
// app.UseAuthorization();

// Classic MVC controllers
// app.MapControllers();

// Minimal API action routes
//app.Logger.LogInformation("Adding Routes");

app.MapHomeRoutes();

//app.MapGroup("/api").MapApiUserRoutes();
app.MapApiUserRoutes();

app.Run();

namespace MyApi
{
    // This line is needed for the test project to work
    public partial class Program
    {
    }
}