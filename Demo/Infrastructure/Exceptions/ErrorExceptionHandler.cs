namespace Demo.Infrastructure.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public sealed class ErrorExceptionHandler(
    ILogger<ErrorExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<ErrorExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct)
    {
        _logger.LogError(
           exception,
           "Unhandled exception for request {Method} {Path}",
           context.Request.Method,
           context.Request.Path
       );

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Title = "Server error",
            Status = 500,
            Detail = "An unexpected error occurred"
        };

        await context.Response.WriteAsJsonAsync(problem, ct);

        return true;
    }
}