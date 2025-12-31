namespace MyApi.Application.Users.GetUser;

public sealed class UserReader(UserRepository repository)
{
    private readonly UserRepository _repository = repository;

    public async Task<UserDto> GetUser(int userId)
    {
        return await _repository.GetUserById(userId);
    }
}