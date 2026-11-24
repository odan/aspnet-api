namespace Demo.Application.Users.CreateUser;

using Demo.Infrastructure.Table;

using SqlKata.Execution;

public sealed class CreateUserRepository(QueryFactory db)
{
    public Task<bool> ExistsUsername(string username, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        var users = new UsersTable();

        return db.Query(users)
            .Where(users.Username, username)
            .ExistsAsync(cancellationToken: ct);
    }

    public async Task<int> InsertUser(string username, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        var users = new UsersTable();

        return await db.Query(users).InsertGetIdAsync<int>(
            Values.Of((users.Username, username)),
            cancellationToken: ct);
    }
}