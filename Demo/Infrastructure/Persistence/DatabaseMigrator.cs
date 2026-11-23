namespace Demo.Infrastructure.Persistence;

using System.Reflection;

using DbUp;

public sealed class DatabaseMigrator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory,
    ILogger<DatabaseMigrator> logger)
{
    public void Migrate()
    {
        var dsn = configuration.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string 'Default'.");

        var upgradeEngine = DeployChanges.To
            .PostgresqlDatabase(dsn)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogTo(loggerFactory)
            .LogScriptOutput()
            .Build();

        var result = upgradeEngine.PerformUpgrade();

        if (!result.Successful)
        {
            logger.LogError(result.Error, "Database migration failed");
            throw result.Error;
        }

        foreach (var script in result.Scripts)
        {
            logger.LogInformation("Applied database script {Script}", script.Name);
        }

        logger.LogInformation("Database migration completed");
    }
}