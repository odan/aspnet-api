using System.Data;
using System.Globalization;

using Demo.Application.Users.CreateUser;
using Demo.Application.Users.DeleteUser;
using Demo.Application.Users.FindUser;
using Demo.Application.Users.GetUser;
using Demo.Application.Users.UpdateUser;
using Demo.Infrastructure.Exceptions;
using Demo.Infrastructure.Localization;
using Demo.Infrastructure.Persistence;
using Demo.Infrastructure.Security;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;

using Npgsql;

using SqlKata.Compilers;
using SqlKata.Execution;

namespace Demo.Infrastructure.Services;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<FindUsersRepository>();
        services.AddScoped<GetUserRepository>();
        services.AddScoped<CreateUserRepository>();
        services.AddScoped<CreateUserValidator>();
        services.AddScoped<UpdateUserRepository>();
        services.AddScoped<UpdateUserValidator>();
        services.AddScoped<DeleteUserRepository>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string 'Default'.");

        services.AddScoped(_ => new NpgsqlConnection(connectionString));
        services.AddScoped<IDbConnection>(serviceProvider =>
            serviceProvider.GetRequiredService<NpgsqlConnection>());
        services.AddScoped(_ => new PostgresCompiler());
        services.AddScoped(serviceProvider => new QueryFactory(
            serviceProvider.GetRequiredService<IDbConnection>(),
            serviceProvider.GetRequiredService<PostgresCompiler>()));
        services.AddScoped<DatabaseMigrator>();
        services.AddScoped<ITransactionFactory, NpgsqlTransactionFactory>();
        services.AddSingleton(TimeProvider.System);

        services
            .AddAuthentication("ApiKey")
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", _ => { });
        services.AddAuthorization(options =>
            options.AddPolicy("ApiKey", policy =>
                policy.AddAuthenticationSchemes("ApiKey").RequireAuthenticatedUser()));

        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<ErrorExceptionHandler>();
        services.AddProblemDetails();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto |
                ForwardedHeaders.XForwardedHost;
            options.ForwardLimit = 1;
        });

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
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes["ApiKey"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "X-API-Key",
                    Description = "Enter the API key configured in the API_KEYS environment variable."
                };

                return Task.CompletedTask;
            });
            options.AddOperationTransformer((operation, context, _) =>
            {
                var requiresApiKey = context.Description.ActionDescriptor.EndpointMetadata
                    .OfType<Microsoft.AspNetCore.Authorization.IAuthorizeData>()
                    .Any(metadata => metadata.Policy == "ApiKey");

                if (requiresApiKey)
                {
                    operation.Security ??= [];
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("ApiKey", context.Document)] = []
                    });
                }

                return Task.CompletedTask;
            });
        });

        return services;
    }
}