namespace Momolith.DeploymentAsCode;

public sealed record DeploymentAsCodeEnabled(bool Enabled);

//IDeploymentAsCode = ContainerRegistry
//Returns a path to the container, which then Deployment module can use

public interface IDeploymentAsCode
{
    internal Task Deploy();
    
    public Task TryDeploy(DeploymentAsCodeEnabled enabled)
    {
        if (enabled.Enabled)
        {
            return Deploy();
        }
        return Task.CompletedTask;
    }
}