namespace MyApi.Infrastructure;

using System;
using System.Globalization;

public sealed class Chronos
{

    private static DateTime? s_dateTime;

    public static DateTime ParseIsoDate(string? input)
    {
        return Parse(input, "yyyy-MM-dd");
    }

    public static DateTime ParseIsoDateTime(string? input)
    {
        return Parse(input, "yyyy-MM-dd HH:mm:ss");
    }

    public static DateTime Parse(string? input, string format)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        if (!DateTime.TryParseExact(
                input,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var result))
        {
            throw new FormatException($"Invalid date: '{input}'");
        }

        return result;
    }

    public static int GetAge(DateTime birthDate)
    {
        var today = Now;
        var age = today.Year - birthDate.Year;

        if (birthDate.AddYears(age) > today)
        {
            age--;
        }

        return age;
    }


    public static void SetTestNow(DateTime dateTime)
    {
        s_dateTime = dateTime;
    }

    public static DateTime Now => s_dateTime ?? DateTime.Now;
}