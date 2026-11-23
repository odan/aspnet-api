namespace Demo.Infrastructure.Table;

public class Field<T>
{
    public Table Table { get; }

    public string Name { get; }

    public string? Alias { get; }

    public Field(Table table, string name, string? alias = null)
    {
        Table = table;
        Name = name;
        Alias = alias;
    }

    public Field<T> As(string alias)
        => new(Table, Name, alias);

    public override string ToString()
    {
        var identifier = $"{Table.Alias ?? Table.Name}.{Name}";

        return Alias == null
            ? identifier
            : $"{identifier} AS {Alias}";
    }

    public static implicit operator string(Field<T> field)
        => field.ToString();
}
