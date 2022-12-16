
using System.Reflection;

public class ServiceCollector
{

    public static void AddNamespaces(IServiceCollection services, Assembly assembly, string ns)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (type.Namespace != null && type.Namespace.StartsWith(ns))
            {
                services.AddScoped(type);
            }
        }
    }

    public static void AddNamespace(IServiceCollection services, Assembly assembly, string ns)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (type.Namespace != null && type.Namespace == ns)
            {
                services.AddScoped(type);
            }
        }
    }
}