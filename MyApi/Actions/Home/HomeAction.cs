namespace MyApi.Actions.Home;

using Microsoft.Extensions.Logging;
using Serilog;

public class HomeAction
{

    private readonly Microsoft.Extensions.Logging.ILogger _logger;

    public HomeAction(ILoggerFactory factory)
    {
        _logger = factory.AddSerilog(
            new LoggerConfiguration()
            .WriteToFile("HomeAction")
            .CreateLogger()
        ).CreateLogger<HomeAction>();
    }

    public string Get()
    {
        _logger.LogInformation("Home action");

        return "Hello, World!";
    }
}
