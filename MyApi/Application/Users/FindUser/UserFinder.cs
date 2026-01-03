namespace MyApi.Application.Users.FindUser;

public sealed class UserFinder(UserFinderRepository repository)
{
    private readonly UserFinderRepository _repository = repository;

    public async Task<List<UserListItem>> FindAllUsers()
    {
        var users = await _repository.FindUsers();

        // Custom logic...

        return users;
    }
}