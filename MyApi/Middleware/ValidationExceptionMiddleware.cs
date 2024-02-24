namespace MyApi.Middleware;

using FluentValidation;
using System.Text.Json;
using System.Net;

public sealed class ValidationExceptionMiddleware(ILoggerFactory factory) : IMiddleware
{
    private readonly ILogger<ValidationExceptionMiddleware> _logger = factory
            .WriteToFile("validation_exception")
            .CreateLogger<ValidationExceptionMiddleware>();

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (ValidationException validationException)
        {
            _logger.LogError(0, validationException, "Validation error");

            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;

            // create list of dynamic objects from a list of objects
            var details = validationException.Errors.Select(error => new
            {
                message = error.ErrorMessage,
                field = error.PropertyName.ToSnakeCase(),
            });

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = new
                {
                    message = validationException.Message,
                    details,
                }
            }));
        }
    }
}

public static class ValidationExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseValidationExceptionMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ValidationExceptionMiddleware>();
    }
}
