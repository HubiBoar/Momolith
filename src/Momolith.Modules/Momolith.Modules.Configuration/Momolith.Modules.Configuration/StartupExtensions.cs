using Explicit.Arguments;
using Microsoft.Extensions.Hosting;
using Momolith.InfrastructureAsCode;

namespace Momolith.Modules.Configuration;

public static class StartupExtensions
{
    public static ConfigurationModule GetConfiguration(
        this IHostApplicationBuilder startup,
        InfrastructureAsCodeEnabled enabled,
        IInfrastructureAsCode<ConfigurationModule> infraAsCode)
    {
        return startup.GetModule(enabled, infraAsCode);
    }
    
    public static ConfigurationModule GetConfiguration<TInfra>(
        this IHostApplicationBuilder startup,
        TInfra infraAsCode)
        where TInfra : IArgumentProvider<InfrastructureAsCodeEnabled>, IInfrastructureAsCode<ConfigurationModule>
    {
        return startup.GetConfiguration(infraAsCode.GetValue(), infraAsCode);
    }
}