var builder = WebApplication.CreateBuilder(args);

//
// Configuration
//

// Load sensitive data from .env file
DotNetEnv.Env.Load();

// Copy specified settings from environment variables
builder.Configuration["DB_DSN"] = DotNetEnv.Env.GetString("DB_DSN");

// Optional: Copy all environment variables to the configuration
//builder.Configuration.AddEnvironmentVariables();

//
// Add services to the DI container
//
builder.Services.AddTransient(provider =>
{
    //var dsn = builder.Configuration.GetConnectionString("Default");
    var dsn = builder.Configuration["DB_DSN"];

    //dsn = "localhost;User ID=root;Password=;Database=my_api";
    return new MySql.Data.MySqlClient.MySqlConnection(dsn);
});

builder.Services.AddTransient(provider =>
{
    var connection = provider.GetRequiredService<MySql.Data.MySqlClient.MySqlConnection>();

    return new SqlKata.Execution.QueryFactory(connection, new SqlKata.Compilers.MySqlCompiler());
});

// Register assembly types by namespace (as scoped)
// Alternatively use: Scrutor or the Q101.ServiceCollectionExtensions
var assembly = System.Reflection.Assembly.GetExecutingAssembly();
MyApp.Support.ServiceCollector.RegisterAssemblyTypesAsScoped(builder.Services, assembly, "MyApi.Domain");

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


// This line is needed for the test project to work
public partial class Program { }
