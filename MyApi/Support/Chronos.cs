namespace MyApi.Support;

using System;

public sealed class Chronos
{

    private static DateTime? _dateTime;

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
        if (DateTime.TryParseExact(
            input,
            format,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AssumeUniversal,
            out var result
        ))
        {
            return result;
        };

        throw new UnexpectedValueException("Invalid date");
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
        _dateTime = dateTime;
    }

    public static DateTime Now => _dateTime ?? DateTime.Now;
}
