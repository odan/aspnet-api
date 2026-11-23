namespace Demo.Application.Users.FindUser;

using Demo.Infrastructure.Table;

using SqlKata.Execution;

public sealed class FindUsersRepository(QueryFactory db)
{
    public async Task<List<FindUsersRow>> FindUsers(CancellationToken ct = default)
    {
        var users = new UsersTable();

        var rows = await db.Query(users)
            .Select(
                users.Id.As("Id"),
                users.Username.As("Username"),
                users.Email.As("Email"))
            .OrderBy(users.Id)
            .GetAsync<FindUsersRow>(cancellationToken: ct);

        return rows.ToList();
    }
}