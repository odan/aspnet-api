using System.Data;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Infrastructure.Persistence;
using SqlKata.Execution;

namespace MyApi.Tests;

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
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

        if (connection.State != ConnectionState.Open)
            connection.Open();

        using var listCommand = connection.CreateCommand();
        listCommand.CommandText =
            """
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = 'public'
              AND table_type = 'BASE TABLE'
              AND table_name <> 'schemaversions'
            """;

        var tableNames = new List<string>();
        using (var reader = listCommand.ExecuteReader())
        {
            while (reader.Read())
            {
                tableNames.Add(reader.GetString(0));
            }
        }

        foreach (var tableName in tableNames)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"TRUNCATE TABLE {QuoteIdentifier(tableName)} RESTART IDENTITY CASCADE;";
            command.ExecuteNonQuery();
        }
    }

    public void InsertUser(string username, string? email = null, DateTime? createdAt = null)
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<QueryFactory>();

        db.Query("users").Insert(new
        {
            username,
            email,
            created_at = createdAt
        });
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

    private static string QuoteIdentifier(string identifier)
    {
        return "\"" + identifier.Replace("\"", "\"\"") + "\"";
    }

    public void Dispose()
    {
        // Intentionally empty. Factory lifetime owns the container/DB.
    }
}
