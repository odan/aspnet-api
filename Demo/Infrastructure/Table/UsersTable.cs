namespace Demo.Infrastructure.Table;

public sealed class UsersTable : Table
{
    public Column<int> Id { get; }

    public Column<string> Username { get; }

    public Column<string?> Email { get; }

    public Column<DateTime?> CreatedAt { get; }

    public UsersTable(string? alias = null)
        : base("users", alias)
    {
        Id = new(this, "id");
        Username = new(this, "username");
        Email = new(this, "email");
        CreatedAt = new(this, "created_at");
    }
}