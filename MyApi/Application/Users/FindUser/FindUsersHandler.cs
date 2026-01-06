namespace MyApi.Application.Users.FindUser;

public sealed class FindUsersHandler(FindUsersRepository repository)
{
    private readonly FindUsersRepository _repository = repository;

    public async Task<FindUsersResult> FindAllUsers()
    {
        var users = await _repository.FindUsers();

        // Custom logic
        // ...

        return new FindUsersResult
        {
            Users = users.Select(user => new UserSummary
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            }).ToList()
        };

    }
}