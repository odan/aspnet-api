using Microsoft.Extensions.DependencyInjection;
using SqlKata.Execution;

namespace MyApi.Tests;

public class TestDatabase : IDisposable
{
    private readonly ApplicationFactory _factory;

    public static bool IsDatabaseDeployed;

    public TestDatabase(ApplicationFactory factory)
    {
        _factory = factory;

        // You can also add common setup logic here that applies to all tests,
        // such as database seeding, client initialization, etc.
        InitDatabase();
    }

    public void ClearTables()
    {
        InitDatabase();

        var db = _factory.Services.GetRequiredService<QueryFactory>();

        db.Statement("truncate table users");
    }

    private void InitDatabase()
    {
        if (IsDatabaseDeployed == true)
        {
            return;
        }

        IsDatabaseDeployed = true;

        var db = _factory.Services.GetRequiredService<QueryFactory>();
        db.Statement("SET unique_checks=0; SET foreign_key_checks=0;");

        dynamic tables = db.Query("information_schema.tables")
            .Select("TABLE_NAME")
            .WhereRaw("table_schema = database()")
            .Get<object>();


        var dropStatements = new StringBuilder();
        foreach (var table in tables)
        {
            dropStatements.AppendFormat("DROP TABLE `{0}`;", table.TABLE_NAME);
        }
        if (dropStatements.Length > 0)
        {
            db.Statement(dropStatements.ToString());

        }

        var sql = File.ReadAllText(Path.Combine("Resources", "schema.sql"));
        db.Statement(sql);

        db.Statement("SET unique_checks=1; SET foreign_key_checks=1;");
    }

    public void InsertFixture(string table, object data)
    {
        var db = _factory.Services.GetRequiredService<QueryFactory>();
        db.Query(table).Insert(data);
    }

    public void Dispose()
    {

    }
}