namespace Demo.Infrastructure.Table;

public class Column<T>
{
    public Table Table { get; }

    public string Name { get; }

    public string? Alias { get; }

    public Column(Table table, string name, string? alias = null)
    {
        Table = table;
        Name = name;
        Alias = alias;
    }

    public Column<T> As(string alias)
        => new(Table, Name, alias);

    public override string ToString()
    {
        var identifier = $"{Table.Alias ?? Table.Name}.{Name}";

        return Alias == null
            ? identifier
            : $"{identifier} AS {Alias}";
    }

    public static implicit operator string(Column<T> column)
        => column.ToString();
}