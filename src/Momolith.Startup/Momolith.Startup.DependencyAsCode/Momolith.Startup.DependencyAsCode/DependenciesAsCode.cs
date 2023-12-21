namespace Momolith.Startup.DependencyAsCode;

public interface IDependencyAsCode
{
}

public interface IDependencyAsCode<TService> : IDependencyAsCode
    where TService : class
{
}

public sealed class Dependency<T>
    where T : class, IDependencyAsCode
{
    internal IServiceProvider Provider { get; }
    
    public Dependency(IServiceProvider provider)
    {
        Provider = provider;
    }
}