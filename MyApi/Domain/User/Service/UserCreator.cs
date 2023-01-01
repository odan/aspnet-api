
namespace MyApi.Domain.User.Service;

using MyApi.Domain.User.Data;
using MyApi.Domain.User.Repository;

public class UserCreator
{
    private readonly UserCreatorRepository repository;

    public UserCreator(UserCreatorRepository repository)
    {
        this.repository = repository;
    }

    public int CreateUser(UserCreatorParameter user)
    {
        var userId = this.repository.InsertUser(user.Username);

        // Logging
        // ...

        return userId;
    }
}

