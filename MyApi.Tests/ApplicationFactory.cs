namespace MyApi.Tests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
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
        // Set the environment to Test
        builder.UseEnvironment("Test");
        // Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureAppConfiguration((ctx, config) =>
        {
            if (!string.Equals(ctx.HostingEnvironment.EnvironmentName, "Test", StringComparison.Ordinal))
                throw new InvalidOperationException("Invalid Test environment");

            // This will clear all previously registered configuration sources.
            config.Sources.Clear();

            // Add configuration sources, e.g., JSON files, environment variables.
            // Load Test settings from appsettings.Test.json
            config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.Test.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();
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
            // Replace MySqlConnection
            services.RemoveAll<MySqlConnection>();

            // Add new SqlConnection
            services.AddScoped(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();

                var dsn = configuration.GetConnectionString("Default")
                          ?? throw new InvalidOperationException("Missing connection string 'Default'.");

                if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
                {
                    dsn = string.Format(
                        "server={0};port={1};uid={2};pwd={3};database={4};AllowUserVariables=True;SslMode=Required;Charset=utf8mb4",
                        Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost",
                        Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306",
                        Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root",
                        Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "root",
                        Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "test"
                    );
                }

                var connection = new MySqlConnection(dsn);
                connection.Open();

                return connection;
            });

        });
    }

}