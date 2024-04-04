// CA1852 Type 'Program' can be sealed because it has no subtypes in its containing assembly and is not externally visible
#pragma warning disable CA1852

using DbUp;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("*** Database Schema Tool ***");
Console.ResetColor();
Console.WriteLine("");

// Test if input arguments were supplied.
if (args.Length == 0)
{
    Console.WriteLine("Please enter a argument.");
    Console.WriteLine("Usage: Schema <command>");
    Console.WriteLine("Example: schema migrate");
    Console.WriteLine("");
    Console.WriteLine("Available commands:");
    Console.WriteLine("- schema migrate <dsn> (Run sql migration scripts)");
    Console.WriteLine("- schema import (Import schema from schema.sql)");
    Console.WriteLine("- schema export (Export schema to schema.sql)");

    return 1;
}

var command = args.FirstOrDefault() ?? "";

// dotnet run -- migrate
// dotnet run -- migrate "Server=localhost;User ID=root;Password=;Database=my_api"

// Load sensitive data from .env file
Console.WriteLine("Current directory:" + Directory.GetCurrentDirectory());

var workingPath = Path.GetDirectoryName(
    System.Reflection.Assembly.GetExecutingAssembly().Location
) ?? "";

var envFile = Path.Combine(workingPath, ".env");

if (File.Exists(envFile))
{
    Console.WriteLine("Loading .env file: " + envFile);
    var fsSource = new FileStream(envFile, FileMode.Open, FileAccess.Read);
    DotNetEnv.Env.Load(fsSource);
}

// Copy sensitive settings from environment variables
var connectionString =
    args.ElementAtOrDefault(1)
    ?? string.Format(
    "Server={0};Port={1};User ID={2};Password={3};Database={4}",
    Environment.GetEnvironmentVariable("MYSQL_HOST") ?? "localhost",
    Environment.GetEnvironmentVariable("MYSQL_PORT") ?? "3306",
    Environment.GetEnvironmentVariable("MYSQL_USER") ?? "root",
    Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "",
    Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? ""
);

if (command == "migrate")
{
    var upgrader =
        DeployChanges.To
            .MySqlDatabase(connectionString)
            .WithScriptsFromFileSystem(Path.Combine(workingPath, "Scripts/"))
            .LogToConsole()
            .Build();

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(result.Error);
        Console.ResetColor();

        return -1;
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Success!");
    Console.ResetColor();

    return 0;
}

if (command == "export")
{
    var schemaSqlFile = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Schema",
        "schema.sql"
    );

    var connection = new MySqlConnection(connectionString);
    try
    {
        Console.WriteLine("Connecting to MySQL...");
        connection.Open();

        var sql = "SELECT database();";
        var cmd = new MySqlCommand(sql, connection);
        var reader = cmd.ExecuteReader();

        reader.Read();
        Console.WriteLine("Use database: " + reader.GetString("database()"));
        reader.Close();

        sql = "SELECT table_name " +
            "FROM information_schema.tables " +
            "WHERE table_schema = database();";

        cmd = new MySqlCommand(sql, connection);
        reader = cmd.ExecuteReader();

        var tableNames = new List<string>();

        while (reader.Read())
        {
            tableNames.Add(reader.GetString("table_name"));
        }
        reader.Close();

        var sqlList = new List<string>();

        foreach (var table in tableNames)
        {
            Console.WriteLine("Table: " + table);

            sql = string.Format("SHOW CREATE TABLE `{0}`;", table);

            reader = new MySqlCommand(sql, connection).ExecuteReader();

            reader.Read();
            var tableSql = reader.GetString("Create Table") + ";";
            tableSql = Regex.Replace(tableSql, "AUTO_INCREMENT=\\d+", "");

            sqlList.Add(tableSql);
            reader.Close();
        }

        Console.WriteLine("Save to file: " + schemaSqlFile);
        File.WriteAllText(schemaSqlFile, string.Join("\n\n", [.. sqlList]));
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.ToString());
        Console.ResetColor();

        return -1;
    }

    connection.Close();

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Success!");
    Console.ResetColor();

    return 0;
}

if (command == "create")
{
    Console.WriteLine("Enter a migration name:");
    var migration = Console.ReadLine();

    if (string.IsNullOrEmpty(migration))
    {
        return -1;
    }

    var sqlFile = Path.Combine(
          Directory.GetCurrentDirectory(),
          "Schema",
          "Scripts",
          string.Format("{0}_{1}.sql", DateTime.Now.ToString("yyyyMMddHHmmss"), migration)
      );

    Console.WriteLine("Create empty file: " + sqlFile);
    File.WriteAllText(sqlFile, "");

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Success!");
    Console.ResetColor();

    return 0;
}

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("Unknown command");
Console.ResetColor();

return -1;