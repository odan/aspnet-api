using DotNetEnv;
using DotNetEnv.Configuration;
using MyApi;
using MyApi.Endpoints;
using MyApi.Infrastructure;
using MyApi.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configuration
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddDotNetEnv(".env", LoadOptions.TraversePath());
}

// Services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApi();

var app = builder.Build();

if (app.Environment.IsDevelopment() || args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
    migrator.Migrate();
}

if (args.Contains("--migrate"))
{
    return;
}

// Middleware
app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRequestLocalization();

// Endpoints
app.MapHomeEndpoints();
app.MapApiUserEndpoints();

app.Run();

namespace MyApi
{
    // This line is needed for the test project to work
    public partial class Program
    {
    }
}
