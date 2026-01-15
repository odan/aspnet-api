namespace MyApi.Infrastructure;

using Microsoft.EntityFrameworkCore;
using MyApi.Infrastructure.Persistence;
using MyApi.Infrastructure.Persistence.Transactions;
using Serilog;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Serilog configuration
        services.AddLogging(logging => logging
            .AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger())
        );

        // Database
        var dsn = configuration.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string 'Default'.");

        services.AddDbContext<AppDbContext>(options =>
        {
            // Oracle/MySQL EF Core provider
            options.UseMySQL(dsn);
        });

        // Transactions
        services.AddScoped<ITransaction, EfTransaction>();

        // services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

}