using Microsoft.Extensions.DependencyInjection;

namespace Momolith.DependencyAsCode;

public static class DependencyContextExtensions
{
    public static TService Get<TService, T>(this Dependency<T> dependencyAsCode)
        where T : class, IDependencyAsCode<TService>
        where TService : class
    {
        return dependencyAsCode.Provider.GetRequiredService<TService>();
    }
}