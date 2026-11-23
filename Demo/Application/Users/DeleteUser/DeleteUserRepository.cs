namespace Demo.Application.Users.DeleteUser;

using Demo.Infrastructure.Table;

using SqlKata.Execution;

public sealed class DeleteUserRepository(QueryFactory db)
{
    public Task<int> DeleteUser(int id, CancellationToken ct = default)
    {
        var users = new UsersTable();

        return db.Query(users)
            .Where(users.Id, id)
            .DeleteAsync(cancellationToken: ct);
    }
}