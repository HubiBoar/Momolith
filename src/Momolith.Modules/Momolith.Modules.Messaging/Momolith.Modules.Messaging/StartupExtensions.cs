using Explicit.Arguments;
using Microsoft.Extensions.Hosting;
using Momolith.InfrastructureAsCode;

namespace Momolith.Modules.Messaging;

public static class StartupExtensions
{
    public static MessagingComponent GetMessaging(
        this IHostApplicationBuilder startup,
        InfrastructureAsCodeEnabled enabled,
        IInfrastructureAsCode<MessagingComponent> infraAsCode)
    {
        return startup.GetModule(enabled, infraAsCode);
    }
    
    public static MessagingComponent GetMessaging<TInfra>(
        this IHostApplicationBuilder startup,
        TInfra infraAsCode)
        where TInfra : IArgumentProvider<InfrastructureAsCodeEnabled>, IInfrastructureAsCode<MessagingComponent>
    {
        return startup.GetModule(infraAsCode.GetValue(), infraAsCode);
    }
}