namespace MyApi.Domain.Customer.Repository;

using SqlKata.Execution;

public sealed class CustomerCreatorRepository(QueryFactory db)
{
    private readonly QueryFactory _db = db;

    public bool ExistsUsername(string username)
    {
        var row = _db.Query("customers")
            .Where("username", username)
            .FirstOrDefault();

        return row != null;
    }

    public int InsertCustomer(string username)
    {
        var userId = _db.Query("customers").InsertGetId<int>(new
        {
            username,
        });

        return userId;
    }
}
