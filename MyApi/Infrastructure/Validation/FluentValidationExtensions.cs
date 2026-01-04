using FluentValidation;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MyApi.Infrastructure.Validation;

public static class FluentValidationExtensions
{
    public static IServiceCollection AddFluentValidation(
           this IServiceCollection services)
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        // Add mapping of JsonPropertyNames
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