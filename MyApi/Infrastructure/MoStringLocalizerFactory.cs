namespace MyApi.Infrastructure;

using Microsoft.Extensions.Localization;

public sealed class MoStringLocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource)
    {
        return new MoStringLocalizer();
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new MoStringLocalizer();
    }
}