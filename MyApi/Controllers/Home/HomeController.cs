namespace MyApi.Controllers.Home;

public sealed class HomeController
{
    public static string Handle(ILoggerFactory factory)
    {
        var logger = factory.CreateLogger<HomeController>();

        logger.LogInformation("Home action");

        return "Hello, World!";
    }
}