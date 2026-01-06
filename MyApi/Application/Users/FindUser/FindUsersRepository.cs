namespace MyApi.Application.Users.FindUser;

using SqlKata.Execution;
using System.Threading;

public sealed class FindUsersRepository(QueryFactory db)
{
    private readonly QueryFactory _db = db;

    public async Task<List<UsersDto>> FindUsers(CancellationToken ct = default)
    {
        var users = await _db
               .Query("users")
               .GetAsync<UsersDto>(cancellationToken: ct);

        return users.ToList();
    }

}