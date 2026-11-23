namespace Demo.Infrastructure.Table;

public sealed class UsersTable : Table
{
    public Field<int> Id { get; }

    public Field<string> Username { get; }

    public Field<string?> Email { get; }

    public Field<DateTime?> CreatedAt { get; }

    public UsersTable(string? alias = null)
        : base("users", alias)
    {
        Id = new(this, "id");
        Username = new(this, "username");
        Email = new(this, "email");
        CreatedAt = new(this, "created_at");
    }
}