namespace MyApi.Application.Users.GetUser;

public sealed class GetUserHandler(GetUserRepository repository)
{
    private readonly GetUserRepository _repository = repository;

    public async Task<UserDto> GetUser(int userId)
    {
        return await _repository.GetUserById(userId);
    }
}