namespace MyApi.Application.Users.GetUser;

using MyApi.Shared.Exceptions;
using SqlKata.Execution;
using System.Threading;

public sealed class UserRepository(QueryFactory db)
{
    private readonly QueryFactory _db = db;

    public async Task<UserDto> GetUserById(int id, CancellationToken ct = default)
    {
        var user = await _db.Query("users")
            .Where("id", id)
            .FirstOrDefaultAsync<UserDto>(cancellationToken: ct);

        return user ?? throw new DomainException("User not found");
    }
}