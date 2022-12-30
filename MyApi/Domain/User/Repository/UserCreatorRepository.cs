namespace MyApi.Domain.User.Repository;

using SqlKata.Execution;

public class UserCreatorRepository
{
    private readonly QueryFactory db;

    public UserCreatorRepository(QueryFactory db)
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
