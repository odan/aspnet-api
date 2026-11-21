using Demo.Api.Application.Home;
using Demo.Api.Application.Users.CreateUser;
using Demo.Api.Application.Users.FindUser;
using Demo.Api.Application.Users.GetUser;
using Demo.Api.Infrastructure.Logging;
using Demo.Api.Infrastructure.Persistence;
using Demo.Api.Infrastructure.Services;
using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuration
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddDotNetEnv(".env", LoadOptions.TraversePath());
}

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});

// Services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<DatabaseMigrator>().Migrate();
}

// Middleware
// Apply forwarded host, scheme, and client IP before other middleware reads the request.
app.UseForwardedHeaders();
app.UseExceptionHandler();
app.UseMiddleware<HttpLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();

// Endpoints
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Demo API v1");
});

app.MapGet("/", HomeHandler.Handle)
    .WithTags("Home");

var api = app.MapGroup("/api")
    .RequireAuthorization("ApiKey")
    .WithMetadata(
        new ProducesResponseTypeMetadata(StatusCodes.Status400BadRequest, typeof(HttpValidationProblemDetails), ["application/problem+json"]),
        new ProducesResponseTypeMetadata(StatusCodes.Status500InternalServerError, typeof(ProblemDetails), ["application/problem+json"]));

var users = api.MapGroup("/users")
    .WithTags("Users");

users.MapGet("/", FindUsersHandler.Handle);

users.MapGet("/{id:int}", GetUserHandler.Handle)
    .WithName(GetUserHandler.RouteName);

users.MapPost("/", CreateUserHandler.Handle)
    .Produces<CreateUserResponse>(StatusCodes.Status201Created);

app.Run();

namespace MyApi
{
    // This line is needed for the test project to work
    public partial class Program
    {
    }
}