namespace MyApi.Support;

using System.Globalization;
using Microsoft.Extensions.Localization;
using NGettext;

public class MoStringLocalizer : IStringLocalizer
{
    private Catalog catalog = new();

    private string cultureName = "en-US";

    // Separator used between the actual key and the plural suffix.
    public string PluralSeparator { get; set; } = "_";

    // Whether to use simple plural suffixes ('plural' for plural and nothing for singular).
    // If set to false all translations would be suffixed with the corresponding plural number.
    public bool UseSimplePluralSuffix { get; set; } = true;

    public LocalizedString this[string name]
    {
        get
        {
            var value = this.GetString(name);
            return new LocalizedString(name, value ?? name, value == null);
        }
    }
    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var actualValue = this[name];
            return !actualValue.ResourceNotFound
                ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
                : actualValue;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings()
    {
        return this.GetAllStrings(true);
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        this.LoadCatalog();

        foreach (var translation in this.catalog.Translations)
        {
            for (var i = 0; i < translation.Value.Length; i++)
            {
                var key = translation.Key;
                var value = translation.Value[i];
                var pluralNumber = i + 1;

                if (this.UseSimplePluralSuffix)
                {
                    if (i == 1)
                    {
                        key += this.PluralSeparator + "plural";
                    }
                    else if (i > 1)
                    {
                        key += this.PluralSeparator + pluralNumber;
                    }
                }
                else
                {
                    key += this.PluralSeparator + pluralNumber;
                }

                yield return new LocalizedString(key, value, false);
            }
        }
    }
    private string GetString(string key)
    {
        this.LoadCatalog();

        return this.catalog.GetString(key);
    }

    private void LoadCatalog()
    {
        var cultureName = Thread.CurrentThread.CurrentCulture.Name;

        // Check of catalog for this language is already loaded
        if (cultureName == this.cultureName)
        {
            return;
        }

        this.cultureName = cultureName;

        var moFile = $"Resources/{cultureName}.mo";

        if (!File.Exists(moFile))
        {
            this.catalog = new Catalog();

            return;
        }

        using (Stream moFileStream = File.OpenRead(moFile))
        {
            this.catalog = new Catalog(
                moFileStream,
                CultureInfo.GetCultureInfo(cultureName)
            );
        }
    }
}


