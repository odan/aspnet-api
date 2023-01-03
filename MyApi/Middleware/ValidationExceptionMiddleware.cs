namespace MyApi.Middleware;

using FluentValidation;

public sealed class ValidationExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (ValidationException validationException)
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.UnprocessableEntity;

            // create list of dynamic objects from a list of objects
            var details = validationException.Errors.Select(error => new
            {
                message = error.ErrorMessage,
                field = error.PropertyName.ToSnakeCase(),
            });

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
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
