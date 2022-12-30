
namespace MyApi.Domain.User.Service;

using FluentValidation;
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
        var result = this.validator.Validate(user);

        if (!result.IsValid)
        {
            throw new ValidationException("Input validation failed", result.Errors);
        }

        var userId = this.repository.InsertUser(user.Username);

        // Logging
        // ...

        return userId;
    }
}

