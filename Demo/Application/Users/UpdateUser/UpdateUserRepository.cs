namespace Demo.Application.Users.UpdateUser;

using Demo.Infrastructure.Table;

using SqlKata.Execution;

public sealed class UpdateUserRepository(QueryFactory db)
{
    public Task<bool> ExistsUsername(string username, int exceptId, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        var users = new UsersTable();

        return db.Query(users)
            .Where(users.Username, username)
            .Where(users.Id, "<>", exceptId)
            .ExistsAsync(cancellationToken: ct);
    }

    public Task<int> UpdateUser(int id, string username, string? email, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        var users = new UsersTable();

        return db.Query(users)
            .Where(users.Id, id)
            .UpdateAsync(new
            {
                username,
                email
            }, cancellationToken: ct);
    }
}