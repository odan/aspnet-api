namespace MyApi.Tests;

using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.InMemory;
using System;

// https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/8.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/CustomWebApplicationFactory.cs

public class ApplicationFactory : ApplicationFactory<Program>
{
}

public class ApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public InMemorySink LoggerEvents { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        DotNetEnv.Env.TraversePath().Load();

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Test";

        if (environmentName == "Test" || environmentName == "CI")
        {
            // Set the environment to Test
            builder.UseEnvironment(environmentName);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environmentName);
        }
        else
        {
            throw new Exception("Invalid Test environment");
        }

        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            string environmentName = hostingContext.HostingEnvironment.EnvironmentName;

            // This will clear all previously registered configuration sources.
            config.Sources.Clear();

            // Add configuration sources, e.g., JSON files, environment variables.
            // Load Test settings from appsettings.Test.json
            config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .AddDotNetEnv(".env", LoadOptions.TraversePath());
        });

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();

            var serilog = new LoggerConfiguration()
                .WriteTo.Sink(LoggerEvents)
                .CreateLogger();

            logging.AddProvider(new SerilogLoggerProvider(serilog, dispose: true));
        });

        builder.ConfigureServices(services =>
        {
            // you can override services here if needed
        });
    }

}