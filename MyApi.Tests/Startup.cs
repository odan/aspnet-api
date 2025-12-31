namespace MyApi.Tests;

using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ApplicationFactory<Program>, ApplicationFactory<Program>>();
        services.AddTransient<ApplicationFactory, ApplicationFactory>();
        services.AddTransient<TestDatabase, TestDatabase>();
        //services.AddTransient<ClientAuth, ClientAuth>();
    }
}