using Microsoft.Extensions.Hosting;
using Momolith.Modules;

namespace Momolith.InfrastructureAsCode;

public static class InfrastructureAsCodeExtensions
{
    public static TModule GetModule<TModule>(
        this IHostApplicationBuilder startup,
        InfrastructureAsCodeEnabled enabled,
        IInfrastructureAsCode<TModule> infrastructureAsCode)
            where TModule : IModule
    {
        return infrastructureAsCode.GetOrProvision(new InfrastructureAsCodeContext<TModule>(startup, enabled));
    }
}