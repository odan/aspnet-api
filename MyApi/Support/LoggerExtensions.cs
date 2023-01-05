namespace MyApi.Support;

using Serilog;
using Serilog.Extensions.Logging;

public static class LoggerExtensions
{
    public static ILoggerFactory WriteToFile(
            this ILoggerFactory factory,
            string name
    )
    {
        var filename = Path.Combine(
            AppContext.BaseDirectory,
            "LogFiles",
            $"{name}_.txt"
        );

        var logger = new LoggerConfiguration()
            .WriteTo.File(
            filename,
            rollingInterval: RollingInterval.Day,
            fileSizeLimitBytes: 10 * 1024 * 1024,
            retainedFileCountLimit: 2,
            rollOnFileSizeLimit: true,
            shared: true,
            flushToDiskInterval: TimeSpan.FromSeconds(1)
        ).CreateLogger();

        factory.AddProvider(new SerilogLoggerProvider(logger));

        return factory;
    }
}

