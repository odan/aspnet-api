namespace Demo.Application.Users.GetUser;

using Demo.Infrastructure.Table;

using SqlKata.Execution;

public sealed class GetUserRepository(QueryFactory db)
{
    public async Task<GetUserByIdRow> GetUserById(int id, CancellationToken ct = default)
    {
        var users = new UsersTable();

        var user = await db.Query(users)
            .Select(
                users.Id.As("Id"),
                users.Username.As("Username"))
            .Where(users.Id, id)
            .FirstOrDefaultAsync<GetUserByIdRow>(cancellationToken: ct);

        return user ?? throw new InvalidDataException("User not found");
    }
}