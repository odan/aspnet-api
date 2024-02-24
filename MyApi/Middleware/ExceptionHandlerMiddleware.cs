namespace MyApi.Middleware;

public sealed class ExceptionHandlerMiddleware(ILoggerFactory factory) : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger = factory
            .WriteToFile("error")
            .CreateLogger<ExceptionHandlerMiddleware>();

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(0, exception, "Internal server error");

            // Handle exception
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
            {
                error = new
                {
                    message = exception?.Message ?? "Error"
                }
            }));
        }
    }
}

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
