namespace Demo.Tests;

using System;

using DotNetEnv;
using DotNetEnv.Configuration;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;

using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.InMemory;

// https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/8.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/CustomWebApplicationFactory.cs

public class ApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private const string TestApiKey = "test-api-key";

    private TestDatabase? _database;

    public InMemorySink Logger { get; } = new();

    public FakeTimeProvider Time { get; } = new();

    public TestDatabase Database
    {
        get
        {
            return _database ??= new TestDatabase((WebApplicationFactory<Program>)(object)this);
        }
    }

    public HttpClient CreateApiClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add("X-API-Key", TestApiKey);
        return client;
    }

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
                .AddDotNetEnv(".env", LoadOptions.TraversePath())
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["API_KEY"] = TestApiKey
                });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<TimeProvider>();
            services.AddSingleton<TimeProvider>(Time);

            services.AddSingleton<ILoggerFactory>(_ =>
            {
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Sink(Logger)
                    .CreateLogger();

                return new SerilogLoggerFactory(logger, dispose: true);
            });
        });
    }

}