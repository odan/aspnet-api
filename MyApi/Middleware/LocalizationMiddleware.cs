namespace MyApi.Middleware;

using System.Globalization;

public class LocalizationMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var cultureKey = context.Request.Headers["Accept-Language"];

        // cultureKey = "de-DE";

        if (!string.IsNullOrEmpty(cultureKey) && this.DoesCultureExist(cultureKey))
        {
            var culture = new CultureInfo(cultureKey);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        await next(context);
    }
    private bool DoesCultureExist(string cultureName)
    {
        return CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Any(culture =>
                string.Equals(
                    culture.Name,
                    cultureName,
                    StringComparison.Ordinal)
            );
    }
}
