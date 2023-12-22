using Momolith.DeploymentAsCode;

namespace Momolith.Modules.Deployment;

public sealed class DeploymentModule : IModule
{
    private readonly IDeploymentAsCode _deploymentAsCode;
    private readonly DeploymentAsCodeEnabled _enabled;

    private DeploymentModule(IDeploymentAsCode deploymentAsCode, DeploymentAsCodeEnabled enabled)
    {
        _deploymentAsCode = deploymentAsCode;
        _enabled = enabled;
    }

    public static DeploymentModule Create(IDeploymentAsCode deploymentAsCode, DeploymentAsCodeEnabled enabled)
    {
        return new DeploymentModule(deploymentAsCode, enabled);
    }

    public Task TryDeploy()
    {
        return _deploymentAsCode.TryDeploy(_enabled);
    }
}