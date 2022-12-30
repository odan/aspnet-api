
namespace MyApi.Domain.User.Service;

using MyApi.Domain.User.Data;
using MyApi.Domain.User.Repository;

public class UserCreator
{
    private readonly UserCreatorRepository repository;
    private readonly UserCreatorValidator validator;

    public UserCreator(
        UserCreatorRepository repository,
        UserCreatorValidator validator
    )
    {
        this.repository = repository;
        this.validator = validator;
    }

    public int CreateUser(UserCreatorParameter user)
    {
        this.validator.Validate(user);

        var userId = this.repository.InsertUser(user.Username);

        // Logging
        // ...

        return userId;
    }
}

