using FluentValidation;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MyApi.Shared.Extensions;

public static class FluentValidationExtensions
{
    public static IServiceCollection AddFluentValidationJsonPropertyNames(
           this IServiceCollection services)
    {
        var defaultResolver = ValidatorOptions.Global.PropertyNameResolver;

        ValidatorOptions.Global.PropertyNameResolver = (type, memberInfo, expression) =>
        {
            var jsonName = memberInfo?
                .GetCustomAttribute<JsonPropertyNameAttribute>()?
                .Name;

            return jsonName ?? defaultResolver(type, memberInfo, expression);
        };

        return services;
    }
}