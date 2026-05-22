namespace MyApi.Application.Users.FindUser;

public sealed class FindUsersHandler(FindUsersRepository repository)
{
    private readonly FindUsersRepository _repository = repository;

    public async Task<FindUsersResponse> FindAllUsers()
    {
        var users = await _repository.FindUsers();

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