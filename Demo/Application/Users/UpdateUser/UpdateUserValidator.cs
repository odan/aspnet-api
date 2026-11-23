namespace Demo.Application.Users.UpdateUser;

using Demo.Application.Util.Validation;

public sealed class UpdateUserValidator(UpdateUserRepository repository)
{
    public async Task Validate(int id, UpdateUserRequest command, CancellationToken ct = default)
    {
        var errors = command.Validate().ThrowIfAny(command);

        var username = command.Username?.Trim();
        if (!string.IsNullOrWhiteSpace(username) &&
            await repository.ExistsUsername(username, id, ct))
        {
            errors.AddError("Username already taken", nameof(command.Username));
        }

        errors.ThrowIfAny(command);
    }
}