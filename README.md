# aspnet-api

A minimal ASP.NET Core API organized as vertical slices in a single project.

## Requirements

* [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
* A PostgreSQL database

## Architecture

`Demo` is the composition root and the only runtime project. Use cases live under `Application`. Technical adapters live under `Infrastructure`. `Program.cs` wires configuration, dependency injection, middleware, and HTTP routes.

### Request flow

1. `Program.cs` maps a route to a static handler method.
2. The handler receives dependencies from DI (validator, repository, logger) plus the request model.
3. Validators run cheap DataAnnotations checks first, then business or database checks. Failures throw `InputValidationException`.
4. Repositories talk to PostgreSQL through a scoped SqlKata `QueryFactory`.
5. The handler returns an `IResult` or a response DTO.

Unhandled exceptions are mapped to Problem Details:

* `InputValidationException` → `400` (`ValidationExceptionHandler`)
* anything else → `500` (`ErrorExceptionHandler`)

`/api` routes require the `ApiKey` authorization policy (`X-API-Key` header). `/` and OpenAPI do not.

### Composition

Startup registers infrastructure first, then application services:

```csharp
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
```

* `AddInfrastructure` registers the PostgreSQL connection, SqlKata, DbUp, API key authentication, exception handlers, forwarded headers, localization, and OpenAPI.
* `AddApplication` registers feature repositories and validators.

Middleware order:

1. Forwarded headers
2. Exception handler
3. HTTP logging
4. Authentication
5. Authorization
6. Request localization

Database migrations run automatically after the host is built.

See [Database transactions](docs/database-transactions.md) for how a handler starts a unit of work and passes `IDbTransaction` into repositories.

## Project Structure

```
Demo/
├── Program.cs                                 # Composition root
├── Application/                               # Use cases (vertical slices)
│   ├── Util/
│   │   └── Validation/
│   │       ├── InputValidationException.cs    # Thrown by validators
│   │       └── ValidationExtensions.cs        # DataAnnotations helpers
│   ├── Home/
│   │   └── HomeHandler.cs
│   └── Users/
│       ├── CreateUser/
│       │   ├── CreateUserHandler.cs
│       │   ├── CreateUserValidator.cs
│       │   ├── CreateUserRepository.cs
│       │   ├── CreateUserRequest.cs
│       │   └── CreateUserResponse.cs
│       ├── FindUsers/
│       │   ├── FindUsersHandler.cs
│       │   ├── FindUsersRepository.cs
│       │   ├── FindUsersResponse.cs
│       │   └── FindUsersRow.cs
│       └── GetUser/
│           ├── GetUserHandler.cs
│           ├── GetUserRepository.cs
│           ├── GetUserResponse.cs
│           └── GetUserByIdRow.cs
├── Infrastructure/                            # Technical adapters
│   ├── Exceptions/
│   │   ├── ValidationExceptionHandler.cs      # 400 Problem Details
│   │   └── ErrorExceptionHandler.cs           # 500 Problem Details
│   ├── Localization/                          # NGettext IStringLocalizer
│   ├── Logging/
│   │   └── HttpLoggingMiddleware.cs
│   ├── Persistence/
│   │   ├── DatabaseMigrator.cs                # DbUp on startup
│   │   ├── ITransactionFactory.cs
│   │   └── NpgsqlTransactionFactory.cs
│   ├── Migrations/                            # Forward-only SQL
│   ├── Schema/
│   │   └── schema.sql                         # Canonical current schema
│   ├── Security/
│   │   └── ApiKeyAuthenticationHandler.cs     # X-API-Key
│   └── Services/
│       └── ServiceExtensions.cs               # AddInfrastructure, AddApplication
└── Resources/
    └── Locale/                                # .po / .mo translations

Demo.Tests/
├── ApplicationFactory.cs                      # Test host
├── TestDatabase.cs
└── Endpoints/                                 # HTTP tests against a test database
    ├── Home/
    └── Users/
```

## Features

* OpenAPI document and Swagger UI
* Minimal API endpoints, handlers, and repositories
* Vertical slices per use case
* Environment-specific configuration
* 12-Factor `.env` configuration loader (for local secrets)
* SqlKata + PostgreSQL
* API key authentication (`X-API-Key`)
* Input validation (DataAnnotations and custom validators)
* Problem Details exception handlers
* Localization (NGettext)
* Context-specific logging (Serilog)
* File-based error logging (Development)
* Database migrations (DbUp)

**Testing**

* xUnit tests
* HTTP endpoint tests (using a test database)
* Assertions for log messages

*Todo*

* Fluent assertions for database tests
* Build script

## Installation

Run the following command to create a new project:

```
git clone https://github.com/odan/aspnet-api.git --depth 1 {my-app-name}
cd {my-app-name}
```

Replace `{my-app-name}` with the desired name for your project.

Create a new PostgreSQL database.

```sql
CREATE DATABASE my_api;
```

Modify the database name accordingly.

Create a `.env` file in the `Demo` directory to override the local development connection string:

```env
ConnectionStrings__Default=Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=my_api
```

Modify the database and credentials accordingly.

In production, set the environment variable `ConnectionStrings__Default` to the connection string. The double underscore maps to the configuration key `ConnectionStrings:Default`.

The `/api` routes require an API key in the `X-API-Key` request header. Set `API_KEY` to one accepted key, or set `API_KEYS` to a semicolon-separated list of accepted keys. Configure these as environment variables in production; for local development, add them to `Demo/.env`.

## Reverse proxy headers

When running behind HAProxy, forward the client address, original scheme, and public host to the application:

```haproxy
option forwardfor
http-request set-header X-Forwarded-Proto https if { ssl_fc }
http-request set-header X-Forwarded-Host %[req.hdr(Host)]
```

ASP.NET Core processes `X-Forwarded-For`, `X-Forwarded-Proto`, and `X-Forwarded-Host` before the rest of the middleware pipeline. It trusts loopback proxies by default, so HAProxy must connect from a loopback address. Forwarded headers from other proxy addresses are ignored unless the application is configured to trust them.

## OpenAPI and Swagger UI

The generated OpenAPI document is JSON at `/openapi/v1.json`. Swagger UI is available at `/swagger` and reads that document. Operations that require the `ApiKey` policy document the `X-API-Key` header.

## Migrations

Migrations are plain SQL files in `Demo/Infrastructure/Migrations` and are executed with DbUp on startup. `Infrastructure/Schema/schema.sql` is the canonical current schema.

### Add migration

Add a new SQL file to `Demo/Infrastructure/Migrations` using a sortable name, for example:

```
002_AddExampleColumn.sql
```

Then run the application. DbUp applies pending scripts automatically.

## Commands

To build and run the application in debug mode, run:

```
dotnet run --project Demo/Demo.csproj
```

or

```
dotnet watch run --project Demo/Demo.csproj
```

Running a release build:

```
dotnet run --project Demo/Demo.csproj --configuration Release
```

Build a project and its dependencies:

```
dotnet build
```

Building a project and its dependencies using Release configuration:

```
dotnet build --configuration Release
```

Cleaning the bin and obj directories:

```
dotnet clean
```

Publish:

```
dotnet publish
```

**Code styles**

Install the `dotnet-format` package:

```
dotnet tool install --global dotnet-format
```

Checking code styles:

```
dotnet format --verify-no-changes
```

Fixing code styles:

```
dotnet format -v d
```

## Translations

Declare the `IStringLocalizer<T>` interface where you need to translate messages.

**Example**

```csharp
public class Example
{
    private readonly IStringLocalizer<Example> _localizer;

    public Example(IStringLocalizer<Example> localizer)
    {
        _localizer = localizer;
    }

    // ...
}
```

### Translation Usage

The default and source language is English. Supported cultures are `en-US` and `de-DE`.

Translating a simple message:

```cs
string text = _localizer.GetString("Hello, World!");

// Output: Hallo, Welt!
```

Translating a message with placeholder(s):

```cs
string text2 = _localizer.GetString("The user {0} logged in", "sally");

// Output: Der Benutzer sally hat sich eingeloggt
```

### Creating a new translation file

* Open Poedit and create a new PO translation file in `Demo/Resources/Locale`.
* The filename must be the same as the culture name, e.g. `de-DE.po`.
* Open the menu `Translations` > `Settings`
* Change the PO language, e.g. `German`
* Add `_localizer.GetString` as sources keyword.
* Add the source paths with your project CS files.
* Save the file and click `Update from source` to parse for new translations.
* Translate the messages and save the file to generate the MO file, e.g. `de-DE.mo`.

### Changing the language

`UseRequestLocalization` selects the culture from the `Accept-Language` header. You can also set it during the request:

```csharp
using System.Globalization;

var culture = new CultureInfo("de-DE");
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;
```

## Testing

For running and viewing test coverage in Visual Studio Code, install these recommended extensions:

- [.NET Core Test Explorer](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer) (`dotnet-test-explorer`) discovers and runs the xUnit tests from the Testing view.
- [Coverage Gutters](https://marketplace.visualstudio.com/items?itemName=ryanluker.vscode-coverage-gutters) (`coverage-gutters`) displays coverage from the LCOV report produced by the configured test arguments.

Create a local **test** database for xUnit.

```sql
CREATE DATABASE my_api_test;
```

Create a `.env` file in the `Demo.Tests` directory:

```env
ConnectionStrings__Default=Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=my_api_test
```

Modify the database and credentials accordingly.

To start the test suite, run:

```
dotnet test
```

## License

The MIT License (MIT). Please see [License File](LICENSE) for more information.
