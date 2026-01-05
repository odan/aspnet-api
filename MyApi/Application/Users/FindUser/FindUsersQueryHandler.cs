namespace MyApi.Application.Users.FindUser;

public sealed class FindUsersQueryHandler(FindUsersRepository repository)
{
    private readonly FindUsersRepository _repository = repository;

    public async Task<List<UserListItem>> FindAllUsers()
    {
        var users = await _repository.FindUsers();

        // Custom logic...

        return users;
    }
}