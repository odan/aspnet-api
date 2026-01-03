namespace MyApi.Application.Users.CreateUser;

using SqlKata.Execution;

public sealed class UserCreatorRepository(QueryFactory db)
{
    private readonly QueryFactory _db = db;

    public async Task<bool> ExistsUsername(string username, CancellationToken ct = default)
    {
        var row = await _db.Query("users")
            .SelectRaw("1")
            .Where("username", username)
            .Limit(1)
            .FirstOrDefaultAsync<int?>(cancellationToken: ct);

        return row != null;
    }

    public async Task<int> InsertUser(string username, CancellationToken ct = default)
    {
        return await _db.Query("users")
            .InsertGetIdAsync<int>(new { username }, cancellationToken: ct);
    }
}