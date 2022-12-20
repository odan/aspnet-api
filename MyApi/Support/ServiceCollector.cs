namespace MyApi.Support;

using System.Reflection;

public class ServiceCollector
{

    // Register assembly types by namespace (as scoped)
    public static void RegisterAssemblyTypesAsScoped(IServiceCollection services, Assembly assembly, string ns)
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
            type.IsAbstract == true ||
            type.BaseType == typeof(Exception))
        {
            return false;
        }

        return true;

        /*
        var constructors = type.GetConstructors();
        if (constructors.Length == 0)
        {
            return true;
        }

        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            foreach (var parameter in parameters)
            {
                {
                    if (parameter.GetType() == typeof(string))
                    {
                        continue;
                    }
                }

            }
        }

        return true;
        */
    }
}
