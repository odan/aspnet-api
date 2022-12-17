
namespace MyApi.Domain.User.Service;

using MyApi.Domain.User.Data;
using MyApi.Domain.User.Repository;

public class UserReader
{
    private readonly UserRepository repository;

    public UserReader(UserRepository repository)
    {
        this.repository = repository;
    }

    public User ReadUser(int id) => this.repository.GetUserById(id);
}

