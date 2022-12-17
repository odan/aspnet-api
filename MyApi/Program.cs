var builder = WebApplication.CreateBuilder(args);

//
// Configuration
//

// Load sensitive data from .env file
DotNetEnv.Env.Load();

// Optional: Copy all environment variables to the configuration
//builder.Configuration.AddEnvironmentVariables();

// Copy specified settings from environment variables
builder.Configuration["DB_DSN"] = DotNetEnv.Env.GetString("DB_DSN");

//
// Add services to the DI container
//
builder.Services.AddTransient(provider =>
{
    //var dsn = builder.Configuration.GetConnectionString("Default");
    var dsn = builder.Configuration["DB_DSN"];

    return new MySql.Data.MySqlClient.MySqlConnection(dsn);
});

// dotnet add package SqlKata
// dotnet add package SqlKata.Execution
builder.Services.AddTransient(provider =>
{
    var connection = provider.GetRequiredService<MySql.Data.MySqlClient.MySqlConnection>();

    return new SqlKata.Execution.QueryFactory(connection, new SqlKata.Compilers.MySqlCompiler());
});

var assembly = System.Reflection.Assembly.GetExecutingAssembly();
MyApp.Support.ServiceCollector.AddNamespaces(builder.Services, assembly, "MyApi.Domain.");

builder.Services.AddControllers().AddControllersAsServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
