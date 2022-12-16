//using Microsoft.AspNetCore.Http;
using System.Reflection;

// dotnet add package MySql.Data
using MySql.Data.MySqlClient;

// dotnet add package SqlKata
// dotnet add package SqlKata.Execution
using SqlKata.Execution;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<Domain.Hello.Service.HelloReader>();
//builder.Services.AddScoped<Domain.Hello.Repository.HelloRepository>();
//builder.Services.AddTransient<ISettingsService, SettingsService>();
//builder.Services.AddTransient<MySqlDbContext>();

//
// Configuration
//
builder.Configuration.AddJsonFile("config/appsettings.json");
var debug = builder.Configuration.GetDebugView();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (environment == "Development")
{
    builder.Configuration.AddJsonFile("config/appsettings.Development.json", optional: true);
}

DotNetEnv.Env.Load();

// Copy all environment variables to the configuration
//builder.Configuration.AddEnvironmentVariables();

builder.Configuration["DB_USERNAME"] = DotNetEnv.Env.GetString("DB_USERNAME");
builder.Configuration["DB_DSN"] = DotNetEnv.Env.GetString("DB_DSN");

//
// DI Container
//
builder.Services.AddTransient(provider =>
{
    //var username = builder.Configuration["DB_USERNAME"];
    //var dsn = builder.Configuration["DB_DSN"];

    var connectionString = builder.Configuration.GetConnectionString("Default");

    return new MySqlConnection(connectionString);
});

builder.Services.AddTransient(provider =>
{
    var connection = provider.GetRequiredService<MySqlConnection>();

    return new QueryFactory(connection, new SqlKata.Compilers.MySqlCompiler());
});

var assembly = Assembly.GetExecutingAssembly();
ServiceCollector.AddNamespaces(builder.Services, assembly, "Domain.");

builder.Services.AddControllers().AddControllersAsServices();

var app = builder.Build();

app.MapControllers();

/*
app.MapGet("/", () =>
{
    return Results.Ok("OK");
});

app.MapPost("/users", () =>
{
    return Results.Ok("OK2");
});
*/

app.Run();
