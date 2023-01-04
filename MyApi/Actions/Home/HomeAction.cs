namespace MyApi.Actions.Home;

using Serilog;

public class HomeAction
{
    private readonly ILogger<HomeAction> _logger;

    public HomeAction(ILoggerFactory factory)
    {
        _logger = factory.AddSerilog(
            new LoggerConfiguration()
            .WriteToFile("home_action")
            .CreateLogger()
        ).CreateLogger<HomeAction>();
    }

    public string Get()
    {
        _logger.LogInformation("Home action");

        return "Hello, World!";
    }
}
