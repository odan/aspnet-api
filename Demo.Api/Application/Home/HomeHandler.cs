namespace Demo.Api.Application.Home;

public sealed class HomeHandler(ILogger<HomeHandler> logger)
{
    private readonly ILogger<HomeHandler> _logger = logger;

    public async Task<IResult> Invoke()
    {
        _logger.LogInformation("Home action");

        return Results.Ok("Hello, World!");
    }
}
