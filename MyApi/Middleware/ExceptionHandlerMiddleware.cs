namespace MyApi.Middleware;

public class ExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
         {
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