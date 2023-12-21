using Explicit.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Momolith.DependencyAsCode;


public static class ServicesCollectionExtensions
{
    private static readonly Type DependencyAsCodeType = typeof(IDependencyAsCode);
    private static readonly Type DependencyAsCodeGenericType = typeof(IDependencyAsCode<>);

    public static IServiceCollection SetupDependencyAsCode(this IServiceCollection collection, string[] args)
    {
        var dependencies = collection
            .Where(x => x.ServiceType.IsAssignableTo(DependencyAsCodeType))
            .SelectMany(type => type.ServiceType
                .GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == DependencyAsCodeGenericType)
                .Select(x => x.GetGenericArguments()[0]))
            .Distinct()
            .ToArray();

        var missingTypes = new List<Type>();

        foreach (var dependency in dependencies)
        {
            if (collection.Any(x => x.ServiceType == dependency) == false)
            {
                missingTypes.Add(dependency);
            }
        }

        if (missingTypes.Count > 0)
        {
            var exceptions = missingTypes
                .Select(x => new Exception($"Missing Type: {x.GetTypeVerboseName()}"))
                .ToArray();

            throw new AggregateException(exceptions);
        }

        collection.AddSingleton(typeof(Dependency<>));

        return collection;
    }
}