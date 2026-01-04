namespace MyApi.Infrastructure.ExceptionHandling;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Wichtig: ValidationException NICHT hier loggen
        if (exception is FluentValidation.ValidationException)
            return false;

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

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}