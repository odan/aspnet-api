# aspnet-api

A minimal ASP.NET Core API

## Requirements

* [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
* A MySQL database

## Features

* OpenAPI
* Dependency injection container (with autowiring)
* Single Action Controllers, Services and Repositories
* Environment specific configuration
* 12-Factor `.env` configuration loader (for sensitive data)
* MySQL database connection
* SQL QueryBuilder (SqlKata)
* Test solution

*Todo*

* Authentication (BasicAuth)
* HTTP endpoint test examples
* Database migrations (DbUp)
* Logging package (Serilog)
* Input validation (FluentValidation)
* ValidationException middleware
* Continuous integration (CI) workflow with GitHub Actions


## Installation

Run the following command to create a new project:

```
git clone https://github.com/odan/aspnet-api.git --depth 1 [my-app-name]/
cd [my-app-name]/
```

Replace `[my-app-name]` with the desired name for your project. 


Create a new MySQL / MariaDB database.

```sql
CREATE DATABASE `my_api`CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci; 

CREATE TABLE `users` (
  `id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(45) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
```

Modify the database name accordingly.

Create a `.env` file in the `MyApi` directory:

```env
DB_DSN=Server=localhost;User ID=root;Password=;Database=my_api
```

Modify the DSN accordingly.

To build and run the application in debug mode, run:

```
dotnet run
```

or

```
dotnet watch run
```

## Commands

Running a release:

```
dotnet run --configuration Release
```

Creating a release build:

```
dotnet build --configuration Release
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
dotnet format
```

## Testing

To start the test suite, run:

```
dotnet test
```

## License

The MIT License (MIT). Please see [License File](LICENSE) for more information.
