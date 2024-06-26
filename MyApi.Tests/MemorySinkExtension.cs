namespace MyApi.Tests;

using Microsoft.Extensions.Logging;

public class TestLoggerFactory(ILoggerFactory factory) : ILoggerFactory
{
    private readonly ILoggerFactory _factory = factory;

    public void AddProvider(ILoggerProvider provider)
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _factory.CreateLogger(categoryName);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}