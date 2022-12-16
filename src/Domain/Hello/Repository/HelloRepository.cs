using SqlKata;
using SqlKata.Execution;

namespace Domain.Hello.Repository;

public class HelloRepository
{
    private readonly QueryFactory db;

    public HelloRepository(QueryFactory db)
    {
        this.db = db;
    }

    public IEnumerable<User> findUser()
    {
        var users = db.Query("users").Get<User>();

        return users;
    }
}

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
}