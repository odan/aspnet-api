namespace MyApi.Infrastructure.Database;

using MySql.Data.MySqlClient;

public sealed class MySqlTransaction(MySqlConnection connection) : ITransaction
{
    private readonly MySqlConnection _connection = connection;

    public void Begin()
    {
        new MySqlCommand("START TRANSACTION;", _connection).ExecuteNonQuery();
    }

    public void Commit()
    {
        new MySqlCommand("COMMIT;", _connection).ExecuteNonQuery();
    }

    public void Rollback()
    {
        new MySqlCommand("ROLLBACK;", _connection).ExecuteNonQuery();
    }
}