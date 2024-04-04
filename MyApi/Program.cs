using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using MyApi.Actions;
using MyApi.Middleware;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

//
// Configuration
//

// Default language
var culture = new CultureInfo("en-US");
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

// Load sensitive data from .env file
DotNetEnv.Env.Load();

//
// Add services to the DI container
//
// Lifetimes
//
// Transient: Creates a new instance of the service every time you request it.
//
// Scoped: creates a new instance for every scope. (Each request is a Scope).
// Within the scope, it reuses the existing service.
//
// Singleton: Creates a new Service only once during the application lifetime,
// and uses it everywhere.

builder.Services.AddScoped(provider =>
{
    var dsn = builder.Configuration.GetConnectionString("Default");

    // Detect github actions
    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_USER")))
    {
        // Copy sensitive settings from environment variables
        // Use caching_sha2_password
        dsn = string.Format(
            "server={0};port={1};uid={2};pwd={3};database={4};AllowUserVariables=True;SslMode=Required;",
            Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost",
            Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306",
            Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root",
            Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "root",
            Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "test"
        );
    }

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


// Register service types by namespace (as scoped)
// Alternatively use: Scrutor or Q101.ServiceCollectionExtensions
var assembly = System.Reflection.Assembly.GetExecutingAssembly();
builder.Services.AddAssemblyScoped(assembly, nameof(MyApi) + "." + nameof(MyApi.Domain));
builder.Services.AddAssemblyScoped(assembly, nameof(MyApi) + "." + nameof(MyApi.Middleware));
builder.Services.AddAssemblyScoped(assembly, nameof(MyApi) + "." + nameof(MyApi.Actions));

// Localization
builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, MoStringLocalizerFactory>();

// Logging
builder.Logging.ClearProviders();
if (isDevelopment)
{
    builder.Logging.AddConsole();
}

builder.Services.Add(ServiceDescriptor.Transient(typeof(ILogger<>), typeof(Logger<>)));
builder.Services.AddTransient<ILoggerFactory, LoggerFactory>();

// The MVC controllers using the Transient lifetime
// builder.Services.AddControllers().AddControllersAsServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Explore the API via Swagger UI http://localhost:<port>/swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapGroup("/api")
    .MapApiCustomerRoutes();

app.Run();

namespace MyApi
{
    // This line is needed for the test project to work
    public partial class Program
    {
    }
}