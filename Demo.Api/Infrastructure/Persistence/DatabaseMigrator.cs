namespace MyApi.Infrastructure.Persistence;

using System.Reflection;
using DbUp;

public sealed class DatabaseMigrator(
    IConfiguration configuration,
    ILogger<DatabaseMigrator> logger)
{
    public void Migrate()
    {
        var dsn = configuration.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string 'Default'.");

        var upgradeEngine = DeployChanges.To
            .PostgresqlDatabase(dsn)
            .WithScriptsEmbeddedInAssembly(
                Assembly.GetExecutingAssembly(),
                name => name.Contains(".Migrations.") && name.EndsWith(".sql"))
            .LogToConsole()
            .Build();

        var result = upgradeEngine.PerformUpgrade();

        if (!result.Successful)
        {
            logger.LogError(result.Error, "Database migration failed");
            throw result.Error;
        }

        logger.LogInformation("Database migration completed");
    }
}
