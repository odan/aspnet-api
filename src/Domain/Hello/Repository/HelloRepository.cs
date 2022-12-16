namespace Domain.Hello.Repository;
using SqlKata.Execution;

public class HelloRepository
{
    private readonly QueryFactory db;

    public HelloRepository(QueryFactory db) => this.db = db;

    public IEnumerable<User> FindUser()
    {
        var users = this.db.Query("users").Get<User>();

        return users;
    }
}

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
}
