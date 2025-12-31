using MyApi.Shared.Extensions;

namespace MyApi.Controllers.Home;

public sealed class HomeController
{
    public static string Handle(ILoggerFactory factory)
    {
        var logger = factory
            .WriteToFile("home_action")
            .CreateLogger(nameof(HomeController));

        logger.LogInformation("Home action");

        return "Hello, World!";
    }
}