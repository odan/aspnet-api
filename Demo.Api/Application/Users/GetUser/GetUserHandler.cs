namespace Demo.Api.Application.Users.GetUser;

public sealed class GetUserHandler(GetUserRepository repository)
{
    public const string RouteName = "GetUser";

    private readonly GetUserRepository _repository = repository;

    public async Task<GetUserResponse> Invoke(int userId)
    {
        var user = await _repository.GetUserById(userId);

        // ...

        return new GetUserResponse
        {
            UserId = user.Id,
            UserName = user.Username,
        };
    }

}
