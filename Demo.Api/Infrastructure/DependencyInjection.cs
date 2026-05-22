namespace Demo.Infrastructure;

using System.Data;
using Demo.Infrastructure.Persistence;
using Npgsql;
using Serilog;
using SqlKata.Compilers;
using SqlKata.Execution;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddLogging(logging => logging
            .AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger())
        );

        var dsn = configuration.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string 'Default'.");

        services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(dsn));
        services.AddScoped(_ => new PostgresCompiler());
        services.AddScoped(sp => new QueryFactory(
            sp.GetRequiredService<IDbConnection>(),
            sp.GetRequiredService<PostgresCompiler>()));
        services.AddScoped<DatabaseMigrator>();

        return services;
    }
}
