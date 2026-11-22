namespace Demo.Application.Users.FindUser;

public static class FindUsersHandler
{
    public static async Task<FindUsersResponse> Handle(
        FindUsersRepository repository,
        CancellationToken ct = default)
    {
        var users = await repository.FindUsers(ct);

        // Custom logic
        // ...

        return new FindUsersResponse
        {
            Users = users.Select(user => new FindUsersItem
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            }).ToList()
        };
    }
}