using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using MyApi.Application.Users.CreateUser;
using MyApi.Application.Users.FindUser;
using MyApi.Application.Users.GetUser;
using MyApi.Infrastructure.Database;
using MyApi.Infrastructure.ExceptionHandling;
using MyApi.Infrastructure.Localization;
using MyApi.Infrastructure.Validation;
using MySql.Data.MySqlClient;
using Serilog;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Globalization;

namespace MyApi;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Serilog configuration
        services.AddLogging(logging => logging
            .AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger())
        );

        // Database
        var dsn = configuration.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string 'Default'.");

        // Connection per request (scoped)
        services.AddScoped(_ => new MySqlConnection(dsn));

        services.AddSingleton<MySqlCompiler>();

        // QueryFactory per request (scoped)
        services.AddScoped(sp =>
        {
            var connection = sp.GetRequiredService<MySqlConnection>();
            var compiler = sp.GetRequiredService<MySqlCompiler>();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            return new QueryFactory(connection, compiler);
        });

        // Transactions
        services.AddScoped<ITransaction, Infrastructure.Database.MySqlTransaction>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Validators
        services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

        // Application Services
        services.AddScoped<UserFinder>();
        services.AddScoped<UserFinderRepository>();

        services.AddScoped<UserReader>();
        services.AddScoped<UserRepository>();

        services.AddScoped<UserCreator>();
        services.AddScoped<UserCreatorRepository>();

        return services;
    }

    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // FluentValidation JSON property-name mapping
        services.AddFluentValidation();

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