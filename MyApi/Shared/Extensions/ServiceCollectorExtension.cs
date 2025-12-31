namespace MyApi.Shared.Extensions;

using Microsoft.AspNetCore.Mvc;
using System.Reflection;

public static class ServiceCollectorExtension
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
            type.BaseType == typeof(Controller) ||
            IsRecord(type))
        {
            return false;
        }

        return true;
    }

    private static bool IsRecord(Type type)
    {
        return type.GetMethod("<Clone>$",
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic) != null;
    }
}