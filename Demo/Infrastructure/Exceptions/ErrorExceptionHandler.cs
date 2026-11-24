namespace Demo.Infrastructure.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public sealed class ErrorExceptionHandler(
    ILogger<ErrorExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct)
    {
        logger.LogError(
            exception,
            "Unhandled exception for request {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Title = "Server error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "An unexpected error occurred",
        };

        await context.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }
}
