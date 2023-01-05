namespace MyApi.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.InMemory;
using SqlKata.Execution;

internal sealed class Application : WebApplicationFactory<Program>
{

    private static InMemorySink _inMemorySink = new();

    public new HttpClient CreateClient()
    {
        return CreateDefaultClient();
    }

    public InMemorySink GetLoggerEvents()
    {
        return _inMemorySink;
    }

    public void ClearTables()
    {
        // var db = Services.GetRequiredService<QueryFactory>();

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<QueryFactory>();

        db.Statement("truncate table users");
    }

    public StringContent CreateJson(object data)
    {
        var json = JsonSerializer.Serialize(data);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Specify the content root directory
        builder.UseContentRoot(Directory.GetCurrentDirectory());

        var dsn = string.Format(
            "Server={0};Port={1};User ID={2};Password={3};Database={4}",
            Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost",
            Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306",
            Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root",
            Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "",
            Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "my_api"
        );

        Environment.SetEnvironmentVariable("DB_DSN", dsn);

        // Command line args
        builder.ConfigureHostConfiguration(config =>
        {
            //config.AddInMemoryCollection(new Dictionary<string, string?> {
            //    ["my_command_line_key"] = "my_command_line_value",
            //});
        });

        // App Configuration
        builder.ConfigureAppConfiguration((hostBuilderContext, config) =>
        {
            //config.AddInMemoryCollection(new Dictionary<string, string?> {
            //    ["MyConfigKey"] = "MyConfigValue",
            //});
        });

        // Config logging
        //builder.ConfigureLogging((hostBuilderContext, loggingBuilder) => { });

        // Use default DI provider
        //builder.UseDefaultServiceProvider((context, options) => { });

        builder.ConfigureServices((hostBuilderContext, services) =>
        {
            // Add or replace dependencies for testing only
            //services.Remove(ServiceDescriptor.Transient<typeof(ILoggerFactory)>);
            //services.Replace(ServiceDescriptor.Transient<ILoggerFactory, FooImplementation>());

            // Create new logger sink for each test.
            // This object contains all log events within a test case.
            _inMemorySink = new InMemorySink();

            // Replace logger factory for testing
            services.AddTransient<ILoggerFactory>((provider) =>
            {
                var configuration = new LoggerConfiguration()
                    .WriteTo.Sink(_inMemorySink);

                var factory = new LoggerFactory();
                factory.AddProvider(new SerilogLoggerProvider(
                    configuration.CreateLogger()
                ));

                // This factory wrapper ensures then the DI container
                // uses this configuration and not the actual program
                // configuration.
                return new TestLoggerFactory(factory);
            }
            );

            // var dsn = hostBuilderContext.Configuration["ConnectionStrings:Default"];
        });

        //var logger = this.Services.GetRequiredService<Type>();

        return base.CreateHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        //connection?.Dispose();

        base.Dispose(disposing);
    }
}
