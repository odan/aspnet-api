using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Demo.Infrastructure.Security;

public sealed class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var expectedKeys = configuration.GetSection("API_KEYS").Get<string[]>()
            ?? configuration["API_KEYS"]?.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            ?? (string.IsNullOrWhiteSpace(configuration["API_KEY"])
                ? Array.Empty<string>()
                : [configuration["API_KEY"]!]);

        if (expectedKeys.Length == 0 ||
            !Request.Headers.TryGetValue("X-API-Key", out var suppliedValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var suppliedBytes = Encoding.UTF8.GetBytes(suppliedValues.ToString());
        var isValid = expectedKeys.Any(expected =>
        {
            var expectedBytes = Encoding.UTF8.GetBytes(expected);
            return expectedBytes.Length == suppliedBytes.Length &&
                   CryptographicOperations.FixedTimeEquals(expectedBytes, suppliedBytes);
        });

        if (!isValid)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
        }

        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, "api-key-client")], Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}