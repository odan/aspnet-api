# aspnet-api

A minimal ASP.NET Core API

## Requirements

* [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
* A MySQL database

## Features

* Controllers, Services and Repositories
* Environment specific configuration
* `.env` configuration loader (for sensitive data)
* MySQL database connection
* SQL QueryBuilder (SqlKata)
* Dependency injection container (with autowiring)

## Installation

Run the following command to create a new project:

```
git clone https://github.com/odan/aspnet-api.git --depth 1 [my-app-name]/
cd [my-app-name]/
```

Replace `[my-app-name]` with the desired name for your project. 

To build and run the application:

```
dotnet run
```

or

```
dotnet watch run
```
## License

The MIT License (MIT). Please see [License File](LICENSE) for more information.
