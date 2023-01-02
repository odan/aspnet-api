namespace MyApi.Domain.Customer.Repository;

using SqlKata.Execution;

public class CustomerCreatorRepository
{
    private readonly QueryFactory db;

    public CustomerCreatorRepository(QueryFactory db)
    {
        this.db = db;
    }

    public bool ExistsUsername(string username)
    {
        var row = this.db.Query("users")
            .Where("username", username)
            .FirstOrDefault();

        return row != null;
    }

    public int InsertUser(string username)
    {
        var userId = this.db.Query("users").InsertGetId<int>(new
        {
            username,
        });

        return userId;
    }
}
