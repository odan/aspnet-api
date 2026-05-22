namespace Demo.Api.Application.Users.CreateUser;

public sealed class CreateUserHandler(
    CreateUserValidator validator,
    //  IStringLocalizer<CreateUserHandler> localizer,
    CreateUserRepository repository,
    ILogger<CreateUserHandler> logger)
{
    private readonly CreateUserValidator _validator = validator;
    //private readonly IStringLocalizer<CreateUserHandler> _localizer = localizer;
    private readonly CreateUserRepository _repository = repository;
    private readonly ILogger<CreateUserHandler> _logger = logger;

    public async Task<IResult> Invoke(CreateUserRequest request, CancellationToken ct = default)
    {
        var result = await Handle(request, ct);

        return Results.CreatedAtRoute(
            GetUser.GetUserHandler.RouteName,
            new { id = result.UserId },
            result);
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken ct = default)
    {
        _logger.LogInformation("Create new user {request}", request);

        // Input validation
        await _validator.Validate(request, ct);

        try
        {
            // Todo: Map command to repository parameter object
            var userId = await _repository.InsertUser(request.Username ?? "", ct);

            // Logging
            _logger.LogInformation("User created. User-ID: {userId}", userId);

            return new CreateUserResponse { UserId = userId };
        }
        catch (Exception exception)
        {
            // Log error
            _logger.LogError(exception, "Failed to create user");

            throw;
        }
    }
}
