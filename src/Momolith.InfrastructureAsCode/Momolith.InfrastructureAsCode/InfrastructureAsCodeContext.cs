using Microsoft.Extensions.Hosting;
using Momolith.Modules;

namespace Momolith.InfrastructureAsCode;

public sealed class InfrastructureAsCodeContext<TModule>
    where TModule : IModule
{
    public IHostApplicationBuilder Startup { get; }

    public bool Provisioning { get; }

    internal InfrastructureAsCodeContext(IHostApplicationBuilder startup, InfrastructureAsCodeEnabled enabled)
    {
        Startup = startup;
        Provisioning = enabled.Enabled;
    }
}