
namespace MyApi.Domain.User.Service;

using MyApi.Domain.User.Data;
using MyApi.Domain.User.Repository;

public class UserFinder
{
    private readonly UserRepository repository;

    public UserFinder(UserRepository repository)
    {
        this.repository = repository;
    }

    public IEnumerable<User> FindAllUsers()
    {
        var users = this.repository.FindUsers();

        // Custom logic...

        return users;
    }
}

