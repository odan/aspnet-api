namespace MyApi.Support;

using Serilog;

public static class LoggerExtensions
{
    public static LoggerConfiguration WriteToFile(
        this LoggerConfiguration logger,
        string name)
    {
        var filename = Path.Combine(
            AppContext.BaseDirectory,
            "LogFiles",
            $"{name}.txt"
        );

        logger.WriteTo.File(
            filename,
            rollingInterval: RollingInterval.Day,
            fileSizeLimitBytes: 10 * 1024 * 1024,
            retainedFileCountLimit: 2,
            rollOnFileSizeLimit: true,
            shared: true,
            flushToDiskInterval: TimeSpan.FromSeconds(1)
        );

        return logger;
    }
}

