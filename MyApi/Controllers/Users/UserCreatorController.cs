namespace MyApi.Controllers.Users;

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyApi.Domain.Exceptions;
using MyApi.Domain.User.Service;
using System.Text.Json;
using System.Text.Json.Serialization;

[ApiController]
public class UserCreatorController : Controller
{
    private readonly UserCreator userCreator;

    public UserCreatorController(UserCreator userCreator)
    {
        this.userCreator = userCreator;
    }

    [Route("api/users")]
    [HttpPost]
    public CreatedResult CreateUser([FromBody] JsonDocument data)
    {
        var user = this.Validate(data);

        //var username = data.RootElement.GetProperty("username").GetString();

        var userId = this.userCreator.CreateUser(user.Username);

        return this.Created("", new { user_id = userId });
    }

    private UserCreatorModel Validate(JsonDocument data)
    {
        var json = data.RootElement.ToString();
        var user = JsonSerializer.Deserialize<UserCreatorModel>(json);

        if (user == null)
        {
            throw new ValidationException("Input required");
        }

        var validator = new UserCreatorValidator();
        var results = validator.Validate(user);

        if (results.IsValid)
        {
            // Check if the username is unique
            //if (await IsUsernameTaken(model.Username))
            //{
            // Throw a ValidationException if the username is not unique
            //throw new ValidationException("Username is already taken.");
            //}
        }

        if (!results.IsValid)
        {
            throw new ValidationException("Input validation failed", results.Errors);
        }

        return user;
    }

    // https://learn.microsoft.com/en-us/answers/questions/1030059/how-to-validate-json-using-c-schema-validation.html
    public class UserCreatorModel
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = "";
    }

    public class UserCreatorValidator : AbstractValidator<UserCreatorModel>
    {
        public UserCreatorValidator()
        {
            RuleFor(user => user.Username)
                .NotNull().WithMessage("Required")
                .NotEmpty().WithMessage("Required")
                .MaximumLength(45).WithMessage("Too long")
                .NotEqual("root").WithMessage("Invalid value");
        }
    }
}

