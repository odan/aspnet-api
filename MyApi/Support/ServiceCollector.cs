namespace MyApi.Support;

using System.Reflection;
using Microsoft.AspNetCore.Mvc;

public static class ServiceCollector
{

    // Register assembly types by namespace (as scoped)
    public static void AddAssemblyScoped(this IServiceCollection services, Assembly assembly, string ns)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (
                type.Namespace != null &&
                type.Namespace.StartsWith(ns, StringComparison.Ordinal) &&
                IsAutowireable(type))
            {
                services.AddScoped(type);
            }
        }
    }

    private static bool IsAutowireable(Type type)
    {
        if (
            type.IsAbstract ||
            type.BaseType == typeof(Exception) ||
            type.BaseType == typeof(Controller))
        {
            return false;
        }

        return true;
    }
}
