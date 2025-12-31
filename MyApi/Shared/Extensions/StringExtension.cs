namespace MyApi.Shared.Extensions;

using System.Globalization;

public static class StringExtension
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return string.Concat(input.Select((c, i) =>
            char.IsUpper(c) && i > 0 && char.IsLower(input[i - 1]) ? "_" + c : c.ToString()))
            .ToLower(CultureInfo.InvariantCulture)
            .Replace(" ", "_");
    }
}