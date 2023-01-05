namespace MyApi.Actions.Home;
public class HomeAction
{
    private readonly ILogger<HomeAction> _logger;

    public HomeAction(ILoggerFactory factory)
    {
        _logger = factory
            .WriteToFile("home_action")
            .CreateLogger<HomeAction>();
    }

    public string Get()
    {
        _logger.LogInformation("Home action");

        return "Hello, World!";
    }
}
