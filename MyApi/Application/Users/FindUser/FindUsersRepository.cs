namespace MyApi.Application.Users.FindUser;

using Microsoft.EntityFrameworkCore;
using MyApi.Infrastructure.Persistence;

public sealed class FindUsersRepository
{
    private readonly AppDbContext _db;

    public FindUsersRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<UsersDto>> FindUsers(CancellationToken ct = default)
    {
        // Read-only query, no change tracking required
        var users = await _db.Users
            .AsNoTracking()
            .Select(u => new UsersDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            })
            .ToListAsync(ct);

        return users;
    }
}