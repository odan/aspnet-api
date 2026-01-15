using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using MyApi.Application.Users.CreateUser;
using MyApi.Application.Users.FindUser;
using MyApi.Application.Users.GetUser;
using MyApi.Infrastructure.ExceptionHandling;
using MyApi.Infrastructure.Localization;
using System.Globalization;

namespace MyApi;

public static class DependencyInjection
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Application Services
        services.AddScoped<FindUsersHandler>();
        services.AddScoped<FindUsersRepository>();

        services.AddScoped<GetUserHandler>();
        services.AddScoped<GetUserRepository>();

        services.AddScoped<CreateUserHandler>();
        services.AddScoped<CreateUserRepository>();
        services.AddScoped<CreateUserValidator>();

        return services;
    }

    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // Exception Handlers
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // Localization
        var defaultCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
        CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("de-DE"),
            };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        services.AddLocalization();
        services.AddSingleton<IStringLocalizerFactory, MoStringLocalizerFactory>();

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}