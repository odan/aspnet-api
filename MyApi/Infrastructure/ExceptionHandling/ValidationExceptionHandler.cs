namespace MyApi.Infrastructure.ExceptionHandling;

using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public sealed class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        _logger.LogWarning(
            exception,
            "Validation failed for request {Method} {Path}",
            context.Request.Method,
            context.Request.Path
        );

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problem = new ValidationProblemDetails(
            validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                ))
        {
            Title = string.IsNullOrEmpty(validationException.Message)
                    ? "Validation failed"
                    : validationException.Message,
            Status = 400
        };

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}