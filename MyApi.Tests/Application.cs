using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlKata.Execution;

namespace MyApi.Tests;

internal class Application : WebApplicationFactory<Program>
{

    public HttpClient CreateClient()
    {

        return CreateDefaultClient();
    }

    public void ClearTables()
    {
        var db = this.Services.GetRequiredService<QueryFactory>();
        db.Statement("truncate table users");
    }

    public StringContent CreateJson(object data)
    {
        string json = JsonSerializer.Serialize(data);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Specify the content root directory
        builder.UseContentRoot(Directory.GetCurrentDirectory());

        var dsn = String.Format(
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
            // services.AddTransient<Type>();
            // services.AddHostedService<FileWriterService>();
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
