var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<Domain.Hello.Service.HelloReader>();
//builder.Services.AddScoped<Domain.Hello.Repository.HelloRepository>();
//builder.Services.AddTransient<ISettingsService, SettingsService>();
//builder.Services.AddTransient<MySqlDbContext>();

//
// Configuration
//
builder.Configuration.Sources.Clear();

// Load default settings
builder.Configuration.AddJsonFile("config/defaults.json");

// Load environment specific settings
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (environment == "Development")
{
    builder.Configuration.AddJsonFile("config/local.dev.json", optional: true);
}

// Load sensitive data from .env file
DotNetEnv.Env.Load();

// Optional: Copy all environment variables to the configuration
//builder.Configuration.AddEnvironmentVariables();

// Copy specified settings from environment variables
builder.Configuration["DB_DSN"] = DotNetEnv.Env.GetString("DB_DSN");

//
// DI Container
//

// dotnet add package MySql.Data
builder.Services.AddTransient(provider =>
{
    var dsn = builder.Configuration.GetConnectionString("Default");
    //var dsn = builder.Configuration["DB_DSN"];

    return new MySql.Data.MySqlClient.MySqlConnection(dsn);
});

// dotnet add package SqlKata
// dotnet add package SqlKata.Execution
builder.Services.AddTransient(provider =>
{
    var connection = provider.GetRequiredService<MySql.Data.MySqlClient.MySqlConnection>();

    return new SqlKata.Execution.QueryFactory(connection, new SqlKata.Compilers.MySqlCompiler());
});

var assembly = System.Reflection.Assembly.GetExecutingAssembly();
App.Support.ServiceCollector.AddNamespaces(builder.Services, assembly, "Domain.");

builder.Services.AddControllers().AddControllersAsServices();

//
// Start
//
var app = builder.Build();

app.MapControllers();

app.Run();
