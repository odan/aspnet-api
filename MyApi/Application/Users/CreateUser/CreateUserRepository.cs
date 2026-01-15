namespace MyApi.Application.Users.CreateUser;

using Microsoft.EntityFrameworkCore;
using MyApi.Domain.Users;
using MyApi.Infrastructure.Persistence;

public sealed class CreateUserRepository
{
    private readonly AppDbContext _db;

    public CreateUserRepository(AppDbContext db) => _db = db;

    public Task<bool> ExistsUsername(string username, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        return _db.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == username, ct);
    }

    public async Task<int> InsertUser(string username, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        var user = new User
        {
            Username = username
            // add more (Email, CreatedAt, etc.)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        // EF sets Id after SaveChanges
        return user.Id;
    }
}