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

    public async Task<int> InsertCustomer(string username)
    {
        var userId = await _db.Query("customers").InsertGetIdAsync<int>(new
        {
            username,
        });

        return userId;
    }
}