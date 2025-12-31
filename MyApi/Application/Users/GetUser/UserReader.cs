namespace MyApi.Application.Users.GetUser;

using MyApi.Application.Users.Data;

public sealed class UserReader(UserRepository repository)
{
    private readonly UserRepository _repository = repository;

    public async Task<User> GetUser(int userId)
    {
        return await _repository.GetUserById(userId);
    }
}