namespace Demo.Infrastructure.Table;

public static class Values
{
    public static Dictionary<string, object> Of(params (Field Column, object? Value)[] values)
    {
        var data = new Dictionary<string, object>();
        foreach (var value in values)
        {
            data[value.Column.Name] = value.Value ?? DBNull.Value;
        }

        return data;
    }

    public static Dictionary<string, object> Set<T>(Field<T> column, T? value)
    {
        return new Dictionary<string, object>().Set(column, value);
    }

    public static Dictionary<string, object> Set(string column, object? value)
    {
        return new Dictionary<string, object>().Set(column, value);
    }

    public static Dictionary<string, object> Set<T>(this Dictionary<string, object> values, Field<T> column, T? value)
    {
        return values.Set(column.Name, value);
    }

    public static Dictionary<string, object> Set(this Dictionary<string, object> values, string column, object? value)
    {
        values[column] = value ?? DBNull.Value;
        return values;
    }
}