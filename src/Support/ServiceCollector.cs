namespace App.Support;

using System.Reflection;

public class ServiceCollector
{

    public static void AddNamespaces(IServiceCollection services, Assembly assembly, string ns)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.Namespace != null && type.Namespace.StartsWith(ns, StringComparison.Ordinal))
            {
                services.AddScoped(type);
            }
        }
    }

    public static void AddNamespace(IServiceCollection services, Assembly assembly, string ns)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.Namespace != null && type.Namespace == ns)
            {
                services.AddScoped(type);
            }
        }
    }
}
