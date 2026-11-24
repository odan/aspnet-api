namespace Demo.Infrastructure.Exceptions;

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using Demo.Application.Util.Validation;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public sealed class ValidationExceptionHandler(
    ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct)
    {
        if (exception is not InputValidationException inputValidationException)
        {
            return false;
        }

        logger.LogWarning(
            exception,
            "Validation failed for request {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var problem = new ValidationProblemDetails(ToErrors(
            inputValidationException.Results,
            inputValidationException.ModelType))
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.Request.Path,
            Title = string.IsNullOrWhiteSpace(inputValidationException.Message)
                ? "Validation failed"
                : inputValidationException.Message,
        };

        await context.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }

    private static Dictionary<string, string[]> ToErrors(
        IList<ValidationResult> results,
        Type? modelType)
    {
        var jsonNameMap = modelType is not null ? BuildJsonNameMap(modelType) : [];
        return results
            .Select(result =>
            {
                var member = result.MemberNames?.FirstOrDefault() ?? "general";
                var message = result.ErrorMessage ?? "Invalid value.";
                var key = jsonNameMap.TryGetValue(member, out var jsonName) ? jsonName : member;
                return new { Key = key, Message = message };
            })
            .GroupBy(item => item.Key)
            .ToDictionary(
                group => group.Key,
                group => group.Select(item => item.Message).Distinct().ToArray());
    }

    private static Dictionary<string, string> BuildJsonNameMap(Type modelType)
    {
        return modelType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(
                property => property.Name,
                property =>
                {
                    var attribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
                    if (attribute is not null)
                    {
                        return attribute.Name;
                    }

                    return JsonNamingPolicy.CamelCase.ConvertName(property.Name);
                });
    }
}
