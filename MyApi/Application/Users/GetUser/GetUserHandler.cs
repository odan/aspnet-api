namespace MyApi.Application.Users.GetUser;

public sealed class GetUserHandler(GetUserRepository repository)
{
    private readonly GetUserRepository _repository = repository;

    public async Task<GetUserResult> GetUser(int userId)
    {
        var user = await _repository.GetUserById(userId);

        // ...

        return new GetUserResult
        {
            UserId = user.Id,
            UserName = user.Username,
        };
    }
}