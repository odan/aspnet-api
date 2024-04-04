namespace MyApi.Actions.Home;

public static class HomeAction
{
    public static string Get(ILoggerFactory factory)
    {
        var logger = factory
            .WriteToFile("home_action")
            .CreateLogger(nameof(HomeAction));

        logger.LogInformation("Home action");

        return "Hello, World!";
    }
}
