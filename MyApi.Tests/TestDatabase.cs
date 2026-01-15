using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Infrastructure.Persistence;

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
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Disable FK checks
        db.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0;");

        var tableNames = db.Database
            .SqlQueryRaw<string>(
                "SELECT TABLE_NAME FROM information_schema.tables WHERE table_schema = DATABASE();")
            .ToList();

        foreach (var name in tableNames)
        {
            // Avoid truncating EF migration history
            if (string.Equals(name, "__EFMigrationsHistory", StringComparison.OrdinalIgnoreCase))
                continue;

            db.Database.ExecuteSqlRaw($"TRUNCATE TABLE `{name}`;");
        }

        // Re-enable FK checks
        db.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1;");
    }

    public void Insert<TEntity>(TEntity entity) where TEntity : class
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Set<TEntity>().Add(entity);
        db.SaveChanges();
    }

    public void Insert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Set<TEntity>().AddRange(entities);
        db.SaveChanges();
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
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Ensure database schema is up-to-date using EF migrations
            db.Database.Migrate();

            _isDatabaseDeployed = true;
        }
    }

    public void Dispose()
    {
        // Intentionally empty. Factory lifetime owns the container/DB.
    }
}