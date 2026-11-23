namespace Demo.Infrastructure.Table;

public abstract class Table
{
    protected Table(string name, string? alias = null)
    {
        Name = name;
        Alias = alias;
    }

    public string Name { get; }

    public string? Alias { get; }

    public override string ToString()
    {
        return Alias == null
            ? Name
            : $"{Name} AS {Alias}";
    }

    public static implicit operator string(Table table)
        => table.ToString();
}