using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using MyApi.Actions;
using MyApi.Middleware;
using MySql.Data.MySqlClient;
using Serilog;
using SqlKata.Compilers;
using SqlKata.Execution;

var builder = WebApplication.CreateBuilder(args);

//
// Configuration
//

// Default language
var culture = new CultureInfo("en-US");
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;

// Load sensitive data from .env file
DotNetEnv.Env.Load();

// Copy sensitive settings from environment variables
builder.Configuration["ConnectionStrings:Default"] = DotNetEnv.Env.GetString("DB_DSN");

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

    return new MySqlConnection(dsn);
});

builder.Services.AddTransient(provider =>
{
    var connection = provider.GetRequiredService<MySqlConnection>();

    return new QueryFactory(connection, new MySqlCompiler());
});

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
app.MapHomeRoutes();

app.MapGroup("/api")
    .MapApiCustomerRoutes();

app.Run();

// This line is needed for the test project to work
public partial class Program
{
}
