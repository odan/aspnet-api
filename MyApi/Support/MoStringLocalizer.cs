namespace MyApi.Support;

using System.Globalization;
using Microsoft.Extensions.Localization;
using NGettext;

public sealed class MoStringLocalizer : IStringLocalizer
{
    private Catalog _catalog = new();

    private string _cultureName = "en-US";

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);

            return new LocalizedString(name, value ?? name, value == null);
        }
    }
    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var actualValue = this[name];

            return !actualValue.ResourceNotFound
                ? new LocalizedString(
                    name,
                    string.Format(CultureInfo.CurrentCulture, actualValue.Value, arguments),
                    false)
                : actualValue;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        LoadCatalog();

        foreach (var translation in _catalog.Translations)
        {
            for (var i = 0; i < translation.Value.Length; i++)
            {
                var key = translation.Key;
                var value = translation.Value[i];

                yield return new LocalizedString(key, value, false);
            }
        }
    }
    private string GetString(string key)
    {
        LoadCatalog();

        return _catalog.GetString(key);
    }

    private void LoadCatalog()
    {
        var cultureName = Thread.CurrentThread.CurrentCulture.Name;

        // Check if catalog for the current language is already loaded
        if (cultureName == _cultureName)
        {
            return;
        }

        _cultureName = cultureName;

        var moFile = $"Resources/{cultureName}.mo";

        if (!File.Exists(moFile))
        {
            _catalog = new Catalog();

            return;
        }

        using (Stream moFileStream = File.OpenRead(moFile))
        {
            _catalog = new Catalog(
                moFileStream,
                CultureInfo.GetCultureInfo(cultureName)
            );
        }
    }
}


