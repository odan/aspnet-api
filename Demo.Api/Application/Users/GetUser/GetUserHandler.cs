namespace Demo.Api.Application.Users.GetUser;

public static class GetUserHandler
{
    public const string RouteName = "GetUser";

    public static async Task<GetUserResponse> Handle(
        GetUserRepository repository,
        int id,
        CancellationToken ct = default)
    {
        var user = await repository.GetUserById(id, ct);

        // ...

        return new GetUserResponse
        {
            UserId = user.Id,
            UserName = user.Username,
        };
    }
}