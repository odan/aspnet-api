namespace MyApi.Support;

using System.Globalization;

public class StringUtil
{
    public static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "";
        }

        return string.Concat(input.Select((c, i) =>
            char.IsUpper(c) && i > 0 && char.IsLower(input[i - 1]) ? "_" + c : c.ToString()))
            .ToLower(CultureInfo.InvariantCulture)
            .Replace(" ", "_");
    }
}
