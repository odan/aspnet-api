using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;
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

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
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