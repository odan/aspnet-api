using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyApi.Tests;

internal class Application : WebApplicationFactory<Program>
{

    public HttpClient CreateClient()
    {

        return CreateDefaultClient();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Specify the content root directory
        builder.UseContentRoot(Directory.GetCurrentDirectory());

        // Host Configuration : Add environment variables starting with DOTNET_
        // and add any command line args passed
        builder.ConfigureHostConfiguration(configurationBuilder => { });

        // App Configuration
        builder.ConfigureAppConfiguration((hostBuilderContext, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DB_DSN"] = "Server=localhost;User ID=root;Password=;Database=my_api",
            });

        });

        // Config logging
        //builder.ConfigureLogging((hostBuilderContext, loggingBuilder) => { });

        // Use default DI provider
        //builder.UseDefaultServiceProvider((context, options) => { });

        builder.ConfigureServices((hostBuilderContext, services) =>
        {
            // services.AddTransient<Type>();
            // services.AddHostedService<FileWriterService>();
            // var dsn = hostBuilderContext.Configuration["DB_DSN"];
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
