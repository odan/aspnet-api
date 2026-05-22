namespace MyApi.Application.Users.FindUser;

using SqlKata.Execution;

public sealed class FindUsersRepository
{
    private readonly QueryFactory _db;

    public FindUsersRepository(QueryFactory db)
    {
        _db = db;
    }

    public async Task<List<FindUsersRow>> FindUsers(CancellationToken ct = default)
    {
        var users = await _db.Query("users")
            .Select("id as Id", "username as Username", "email as Email")
            .OrderBy("id")
            .GetAsync<FindUsersRow>();

        return users.ToList();
    }
}
