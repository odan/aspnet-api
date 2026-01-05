namespace MyApi.Controllers.Users.GetUser;

using MyApi.Application.Users.GetUser;

public static class GetUserController
{
    public static async Task<GetUserResponse> Handle(GetUserCommandHandler userReader, int id)
    {
        var user = await userReader.GetUser(id);

        var response = new GetUserResponse
        {
            UserId = user.Id,
            UserName = user.Username,
        };

        return response;

    }
}