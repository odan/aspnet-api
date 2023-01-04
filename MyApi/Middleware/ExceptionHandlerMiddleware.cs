namespace MyApi.Middleware;

using Serilog;

public sealed class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILoggerFactory factory)
    {
         _logger = factory.AddSerilog(
            new LoggerConfiguration()
            .WriteToFile("error")
            .CreateLogger()
        ).CreateLogger<ExceptionHandlerMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(0, exception, "General error");

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
