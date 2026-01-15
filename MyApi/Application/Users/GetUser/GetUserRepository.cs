namespace MyApi.Application.Users.GetUser;

using Microsoft.EntityFrameworkCore;
using MyApi.Infrastructure.Persistence;
using System.Threading;

public sealed class GetUserRepository
{
    private readonly AppDbContext _db;

    public GetUserRepository(AppDbContext db) => _db = db;

    public async Task<UserDto> GetUserById(int id, CancellationToken ct = default)
    {
        var user = await _db.Users
          .AsNoTracking()
          .Where(u => u.Id == id)
          .Select(u => new UserDto
          {
              Id = u.Id,
              Username = u.Username,
              //Email = u.Email
          })
          .FirstOrDefaultAsync(ct);

        return user ?? throw new InvalidDataException("User not found");
    }
}