namespace Demo.Api.Application.Users.GetUser;

using System.Threading;
using SqlKata.Execution;

public sealed class GetUserRepository
{
    private readonly QueryFactory _db;

    public GetUserRepository(QueryFactory db) => _db = db;

    public async Task<GetUserByIdRow> GetUserById(int id, CancellationToken ct = default)
    {
        var user = await _db.Query("users")
            .Select("id as Id", "username as Username")
            .Where("id", id)
            .FirstOrDefaultAsync<GetUserByIdRow>();

        return user ?? throw new InvalidDataException("User not found");
    }
}
