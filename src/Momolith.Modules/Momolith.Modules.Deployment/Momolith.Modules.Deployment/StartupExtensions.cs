using Explicit.Arguments;
using Microsoft.Extensions.Hosting;
using Momolith.InfrastructureAsCode;

namespace Momolith.Modules.Deployment;

public static class StartupExtensions
{
    public static Task DeployCode(
        this IHostApplicationBuilder startup,
        InfrastructureAsCodeEnabled enabled,
        IInfrastructureAsCode<DeploymentModule> infraAsCode)
    {
        return startup.GetModule(enabled, infraAsCode).TryDeploy();
    }
    
    public static Task DeployCode<TInfra>(
        this IHostApplicationBuilder startup,
        TInfra infraAsCode)
        where TInfra : IArgumentProvider<InfrastructureAsCodeEnabled>, IInfrastructureAsCode<DeploymentModule>
    {
        return startup.GetModule(infraAsCode.GetValue(), infraAsCode).TryDeploy();
    }
}