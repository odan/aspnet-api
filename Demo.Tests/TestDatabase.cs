using Demo.Api.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Execution;

namespace Demo.Tests;

public sealed class TestDatabase : IDisposable
{
    private readonly ApplicationFactory _factory;

    private static readonly object InitLock = new();

    private static bool _isDatabaseDeployed = false;

    public TestDatabase(ApplicationFactory factory)
    {
        _factory = factory;
        InitDatabase();
    }

    public void ClearTables()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<QueryFactory>();

        var tableNames = db.Query("information_schema.tables")
            .Select("table_name")
            .Where("table_schema", "public")
            .Where("table_type", "BASE TABLE")
            .Where("table_name", "<>", "schemaversions")
            .Get<string>()
            .ToList();

        foreach (var tableName in tableNames)
        {
            db.Statement($"TRUNCATE TABLE {db.Compiler.Wrap(tableName)} RESTART IDENTITY CASCADE");
        }
    }

    private void InitDatabase()
    {
        if (_isDatabaseDeployed)
            return;

        lock (InitLock)
        {
            if (_isDatabaseDeployed)
                return;

            using var scope = _factory.Services.CreateScope();
            var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
            migrator.Migrate();

            _isDatabaseDeployed = true;
        }
    }

    public void Dispose()
    {
        // Intentionally empty. Factory lifetime owns the container/DB.
    }
}