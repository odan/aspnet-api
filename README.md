# aspnet-api

A minimal ASP.NET Core API

## Requirements

* [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* A MySQL database

## Features

* OpenAPI
* Dependency injection container
* Single Action Controllers, Services and Repositories
* Environment specific configuration
* 12-Factor `.env` configuration loader (for sensitive data)
* MySQL database connection
* SQL QueryBuilder (SqlKata)
* Database transaction handling
* Input validation (FluentValidation)
* ValidationException middleware
* Localization (NGettext)
* Context specific logging (Serilog)
* File based error logging
* Continuous integration (CI) workflow with GitHub Actions
* Database migrations (DbUp)

**Testing**

* XUnit tests
* HTTP endpoint tests (using a test database)
* Fluent assertions for log messages

*Todo*

* Fluent assertions for database tests 
* Authentication (BasicAuth)
* Build script

## Installation

Run the following command to create a new project:

```
git clone https://github.com/odan/aspnet-api.git --depth 1 {my-app-name}
cd {my-app-name}
```

Replace `{my-app-name}` with the desired name for your project. 


Create a new MySQL / MariaDB database.

```sql
CREATE DATABASE `my_api` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci; 
```

Modify the database name accordingly.

Create a `.env` file in the `MyApi` directory:

```env
ConnectionStrings__Default=server=127.0.0.1;uid=root;pwd=;database=my_api
```

Modify the database and credentials accordingly.

Import the [schema.sql](Schema/schema.sql) file manually or 
run the following command:

```
npm run migration:migrate
```

**Note:** This command requires [NPM](https://nodejs.org/en/download/).

## Commands

To build and run the application in debug mode, run:

```
dotnet run
```

or

```
dotnet watch run
```

Running a release build:

```
dotnet run --configuration Release
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

Clean and build:

```
dotnet rebuild
```

Publish:

```
dotnet publish
```

**Code styles**

Install the `dotnet-format ` package:

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

Declare the `IStringLocalizer<T>` interface 
where you need to translate messages.

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

The default and source language is english.

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

* Open Poedit and create a new PO translation file in the project `Resources` directory.
* The filename must be the same as the culture name, e.g. `de-DE.po`.
* Open the menu `Translations` > `Settings`
* Change the PO language, e.g. `German`
* Add `_localizer.GetString` as sources keyword.
* Add the source paths with your project CS files.
* Save the file and click `Update from source` to parse for new translations.
* Translate the messages and save the file to generate the MO file, e.g. `de-DE.mo`.

### Changing the language

You can change the language during the request by setting the CurrentCulture
as follows:

```csharp
using using System.Globalization;
// ...

var culture = new CultureInfo("de-DE");
Thread.CurrentThread.CurrentCulture = culture;
Thread.CurrentThread.CurrentUICulture = culture;
```

The `LocalizationMiddleware` detects the user language using the HTTP request
`Accept-Language` header value. If this header contains a valid code, the 
CurrentCulture will be switched automatically.

## Testing

Create a local **test** database for xUnit.

```sql
CREATE DATABASE `my_api_test` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci; 
```

Create a `.env` file in the `MyApi.Tests` directory:

```env
ConnectionStrings__Default=server=127.0.0.1;uid=root;pwd=;database=my_api_test
```

Modify the database and credentials accordingly.

To start the test suite, run:

```
dotnet test
```

## License

The MIT License (MIT). Please see [License File](LICENSE) for more information.
