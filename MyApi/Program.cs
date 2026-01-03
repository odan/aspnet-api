using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.Extensions.Options;
using MyApi;
using MyApi.Endpoints;
using MyApi.Middleware;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configuration
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddDotNetEnv(".env", LoadOptions.TraversePath());
}

// Services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi();

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();
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