namespace MyApi.Application.Users.CreateUser;

using SqlKata.Execution;

public sealed class UserCreatorRepository(QueryFactory db)
{
    private readonly QueryFactory _db = db;

    public bool ExistsUsername(string username)
    {
        var row = _db.Query("users")
            .Where("username", username)
            .FirstOrDefault();

        return row != null;
    }

    public async Task<int> InsertUser(string username)
    {
        var userId = await _db.Query("users").InsertGetIdAsync<int>(new
        {
            username,
        });

        return userId;
    }
}