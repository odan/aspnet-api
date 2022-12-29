namespace MyApi.Support;

using System;

public class Chronos
{

    private static DateTime? dateTime;

    public static DateTime ParseIsoDate(string input)
    {
        return Chronos.Parse(input, @"yyyy-MM-dd");
    }

    public static DateTime ParseIsoDateTime(string input)
    {
        return Chronos.Parse(input, @"yyyy-MM-dd HH:mm:ss");
    }

    public static DateTime Parse(string input, string format)
    {
        DateTime result;
        if (DateTime.TryParseExact(
            input,
            format,
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AssumeUniversal,
            out result
        ))
        {
            return result;
        };

        throw new Exception("Invalid date");
    }

    public static int GetAge(DateTime birthDate)
    {
        DateTime today = Chronos.Now;

        int age = today.Year - birthDate.Year;
        if (birthDate.AddYears(age) > today)
        {
            age--;
        }
        return age;
    }


    public static void SetTestNow(DateTime dateTime)
    {
        Chronos.dateTime = dateTime;
    }

    public static DateTime Now
    {
        get
        {
            return Chronos.dateTime ?? DateTime.Now;
        }
    }
}
