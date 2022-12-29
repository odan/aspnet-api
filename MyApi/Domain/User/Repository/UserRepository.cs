namespace MyApi.Domain.User.Repository;

using MyApi.Domain.Exceptions;
using MyApi.Domain.User.Data;
using SqlKata.Execution;

public class UserRepository
{
    private readonly QueryFactory db;

    public UserRepository(QueryFactory db)
    {
        this.db = db;
    }

    public IEnumerable<User> FindUsers()
    {
        return this.db.Query("users").Get<User>();
    }

    public User GetUserById(int id)
    {
        var user = this.db.Query("users").Where("id", id).FirstOrDefault<User>();

        if (user == null)
        {
            throw new DomainException("User not found");
        }

        return user;
    }

    public int InsertUser(string username)
    {
        var userId = this.db.Query("users").InsertGetId<int>(new
        {
            username,
        });

        return userId;
    }
}
