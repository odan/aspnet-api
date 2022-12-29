using MyApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

//
// Configuration
//

// Load sensitive data from .env file
DotNetEnv.Env.Load();

// Copy sensitive settings from environment variables
builder.Configuration["ConnectionStrings:Default"] = DotNetEnv.Env.GetString("DB_DSN");

//
// Add services to the DI container
//
builder.Services.AddTransient(provider =>
{
    var dsn = builder.Configuration.GetConnectionString("Default");

    return new MySql.Data.MySqlClient.MySqlConnection(dsn);
});

builder.Services.AddTransient(provider =>
{
    var connection = provider.GetRequiredService<MySql.Data.MySqlClient.MySqlConnection>();

    return new SqlKata.Execution.QueryFactory(connection, new SqlKata.Compilers.MySqlCompiler());
});

// Register service types by namespace (as scoped)
// Alternatively use: Scrutor or the Q101.ServiceCollectionExtensions
var assembly = System.Reflection.Assembly.GetExecutingAssembly();
MyApi.Support.ServiceCollector.RegisterAssemblyTypesAsScoped(builder.Services, assembly, "MyApi.Domain");
MyApi.Support.ServiceCollector.RegisterAssemblyTypesAsScoped(builder.Services, assembly, "MyApi.Middleware");

// builder.Services.AddTransient<...>();

builder.Services.AddControllers().AddControllersAsServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add error handler middlewares
app.UseExceptionHandlerMiddleware();
app.UseValidationExceptionMiddleware();

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
