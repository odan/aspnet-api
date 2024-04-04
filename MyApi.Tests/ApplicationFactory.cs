namespace MyApi.Tests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.InMemory;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/8.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/CustomWebApplicationFactory.cs

public class ApplicationFactory : ApplicationFactory<MyApi.Program> { }

public class ApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private static InMemorySink _inMemorySink = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set the environment to Test
        builder.UseEnvironment("Test");
        // Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            string environmentName = hostingContext.HostingEnvironment.EnvironmentName;
            if (environmentName != "Test")
            {
                throw new Exception("Invalid Test environment");
            }

            // This will clear all previously registered configuration sources.
            config.Sources.Clear();

            // Add configuration sources, e.g., JSON files, environment variables.
            // Load Test settings from appsettings.Test.json
            config
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        });

        builder.ConfigureServices(services =>
        {
            // Remove MySqlConnection
            services.RemoveAll(typeof(MySqlConnection));

            // Add new SqlConnection
            services.AddScoped(container =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                // Get database connection string for test environment
                var dsn = configuration.GetConnectionString("Default");

                // Detect github actions
                if (string.IsNullOrEmpty(dsn))
                {
                    // Copy sensitive settings from environment variables
                    // Use caching_sha2_password
                    dsn = string.Format(
                        "server={0};port={1};uid={2};pwd={3};database={4};AllowUserVariables=True;SslMode=Required;Charset=utf8mb4",
                        Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost",
                        Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306",
                        Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root",
                        Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "root",
                        Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "test"
                    );
                }

               // if (string.IsNullOrEmpty(dsn))
               // {
                 //   dsn = "server=localhost;port=3306;uid=root;pwd=root;database=test;AllowUserVariables=True;SslMode=Required;Charset=utf8mb4";
                // }

                // Change DSN to test database
                var connection = new MySqlConnection(dsn);
                connection.Open();

                return connection;
            });

            // Create new logger sink for each test.
            // This object contains all log events within a test case.
            _inMemorySink = new InMemorySink();

            // Remove service desriptors.
            // This method does not remove all the instances.
            services.RemoveAll(typeof(ILoggerFactory));

            services.AddTransient<ILoggerFactory>((provider) =>
            {
                var configuration = new LoggerConfiguration()
                    .WriteTo.Sink(_inMemorySink);

                var factory = new LoggerFactory();

                // The SerilogLoggerProvider is not designed for DI because it
                // it will be bounded to the global state in Log.CloseAndFlush
                // to expose the loggers.
                factory.AddProvider(new SerilogLoggerProvider(
                    configuration.CreateLogger(),
                    // If true, the logger will be disposed/closed
                    // when the provider is disposed.
                    true
                ));

                // This factory wrapper converts the Serilog logger
                // to an instance of Microsoft.Extensions.Logging.ILogger
                // and ensures that only this InMemory logger will be used.
                return new TestLoggerFactory(factory);
            });

        });
    }

    public StringContent CreateJson(object data)
    {
        var json = JsonSerializer.Serialize(data);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    public InMemorySink GetLoggerEvents()
    {
        return _inMemorySink;
    }

}