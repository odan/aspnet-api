
namespace MyApi.Database;

using MySql.Data.MySqlClient;

public sealed class Transaction : ITransaction
{
    private readonly MySqlConnection _connection;

    public Transaction(MySqlConnection connection)
    {
        _connection = connection;
    }

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
