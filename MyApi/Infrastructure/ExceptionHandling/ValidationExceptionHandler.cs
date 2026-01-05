namespace MyApi.Infrastructure.ExceptionHandling;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyApi.Application.Common.Validation;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class ValidationExceptionHandler(
    ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not InputValidationException inputValidationException)
        {
            return false;
        }

        _logger.LogWarning(
            exception,
            "Validation failed for request {Method} {Path}",
            context.Request.Method,
            context.Request.Path
        );

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = ToErrors(
            inputValidationException.Results,
            inputValidationException.ModelType
        );

        var problem = new ValidationProblemDetails(errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.Request.Path,
            Title = string.IsNullOrWhiteSpace(inputValidationException.Message)
                ? "Validation failed"
                : inputValidationException.Message,
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static Dictionary<string, string[]> ToErrors(
        IList<ValidationResult> results,
        Type? modelType)
    {
        var jsonNameMap = modelType is not null ? BuildJsonNameMap(modelType) : [];

        return results
            .Select(r =>
            {
                var member = r.MemberNames?.FirstOrDefault() ?? "general";
                var message = r.ErrorMessage ?? "Invalid value.";
                var key = jsonNameMap.TryGetValue(member, out var jsonName) ? jsonName : member;

                return new { Key = key, Message = message };
            })
            .GroupBy(x => x.Key)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Message).Distinct().ToArray()
            );
    }

    private static Dictionary<string, string> BuildJsonNameMap(Type modelType)
    {
        return modelType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(
                p => p.Name,
                p =>
                {
                    var attr = p.GetCustomAttribute<JsonPropertyNameAttribute>();

                    // Use JsonPropertyName if present
                    if (attr is not null)
                    {
                        return attr.Name;
                    }

                    // Fallback to API naming policy
                    return JsonNamingPolicy.CamelCase.ConvertName(p.Name);
                }
            );
    }
}