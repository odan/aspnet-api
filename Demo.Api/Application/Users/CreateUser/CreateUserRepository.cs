namespace Demo.Api.Application.Users.CreateUser;

using SqlKata.Execution;

public sealed class CreateUserRepository
{
    private readonly QueryFactory _db;

    public CreateUserRepository(QueryFactory db) => _db = db;

    public Task<bool> ExistsUsername(string username, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        return _db.Query("users")
            .Where("username", username)
            .ExistsAsync();
    }

    public async Task<int> InsertUser(string username, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        return await _db.Query("users").InsertGetIdAsync<int>(new
        {
            username
        });
    }
}
