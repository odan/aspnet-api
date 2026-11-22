namespace Demo.Infrastructure.Persistence;

using System.Reflection;

using DbUp;
using DbUp.Engine;

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
            .WithScripts(GetMigrationScripts())
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

    private static IEnumerable<SqlScript> GetMigrationScripts()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string marker = ".Migrations.";
        var scripts = new List<SqlScript>();

        foreach (var resourceName in assembly.GetManifestResourceNames()
            .Where(name => name.Contains(marker, StringComparison.Ordinal) &&
                           name.EndsWith(".sql", StringComparison.Ordinal))
            .OrderBy(name => name, StringComparer.Ordinal))
        {
            var scriptName = resourceName[
                (resourceName.LastIndexOf(marker, StringComparison.Ordinal) + marker.Length)..];

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null)
            {
                continue;
            }

            using var reader = new StreamReader(stream);
            scripts.Add(new SqlScript(scriptName, reader.ReadToEnd()));
        }

        return scripts;
    }
}