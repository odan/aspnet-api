namespace MyApi.Tests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.InMemory;
using SqlKata.Execution;

internal sealed class Application : WebApplicationFactory<Program>
{

    private static InMemorySink _inMemorySink = new();

    private static bool _databaseSetup;

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
        InitDatabaseSchema();
        // var db = Services.GetRequiredService<QueryFactory>();

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<QueryFactory>();

        db.Statement("truncate table customers");
    }

    private void InitDatabaseSchema()
    {
        if (_databaseSetup)
        {
            return;
        }

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<QueryFactory>();

        db.Statement("SET unique_checks=0; SET foreign_key_checks=0;");

        dynamic tables = db.Query("information_schema.tables")
            .Select("TABLE_NAME")
            .WhereRaw("table_schema = database()")
            .Get<object>();


        var dropStatements = new StringBuilder();
        foreach (var table in tables)
        {
            dropStatements.AppendFormat("DROP TABLE `{0}`;", table.TABLE_NAME);
        }
        if (dropStatements.Length > 0)
        {
            db.Statement(dropStatements.ToString());

        }

        var sql = File.ReadAllText(Path.Combine("Resources", "schema.sql"));
        db.Statement(sql);

        db.Statement("SET unique_checks=1; SET foreign_key_checks=1;");

        _databaseSetup = true;
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

        var dir = Directory.GetCurrentDirectory();

        // Load .env file within the test project
        DotNetEnv.Env.Load();

        var dsn = string.Format(
            "Server={0};Port={1};User ID={2};Password={3};Database={4}",
            Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost",
            Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306",
            Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root",
            Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "",
            Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? ""
        );

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

            // Optional: Clear all logs
            // Log.CloseAndFlush();

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
