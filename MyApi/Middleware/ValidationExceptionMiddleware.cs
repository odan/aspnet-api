namespace MyApi.Middleware;

using FluentValidation;
using MyApi.Shared.Extensions;
using System.Net;
using System.Text.Json;

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
            _logger.LogError(400, validationException, "Validation error");

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            // create list of dynamic objects from a list of objects
            var errors = new Dictionary<string, string[]>();
            foreach (var error in validationException.Errors)
            {
                errors.Add(error.PropertyName.ToSnakeCase(), [error.ErrorMessage]);
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                title = string.IsNullOrEmpty(validationException.Message) ?
                    "One or more validation errors occurred." :
                    validationException.Message,
                status = context.Response.StatusCode,
                errors
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