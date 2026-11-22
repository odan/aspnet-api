namespace Demo.Application.Home;

public static class HomeHandler
{
    public static IResult Handle(ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(nameof(HomeHandler));
        logger.LogInformation("Home action");

        return Results.Ok("Hello, World!");
    }
}