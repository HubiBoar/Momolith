using Momolith.Modules;

namespace Momolith.InfrastructureAsCode;

public interface IInfrastructureAsCode<TModule>
    where TModule : IModule
{
    public TModule GetOrProvision(InfrastructureAsCodeContext<TModule> context);
}