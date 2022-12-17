
namespace MyApi.Domain.User.Service;

using MyApi.Domain.User.Repository;

public class UserCreator
{
    private readonly UserRepository repository;

    public UserCreator(UserRepository repository)
    {
        this.repository = repository;
    }

    public int CreateUser(string username)
    {
        var userId = this.repository.InsertUser(username);

        // Logging
        // ...

        return userId;
    }
}

