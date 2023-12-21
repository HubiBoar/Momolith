using Explicit.Arguments;
using Microsoft.Extensions.Hosting;
using Momolith.InfrastructureAsCode;

namespace Momolith.Modules.Messaging;

public static class StartupExtensions
{
    public static MessagingModule GetMessaging(
        this IHostApplicationBuilder startup,
        InfrastructureAsCodeEnabled enabled,
        IInfrastructureAsCode<MessagingModule> infraAsCode)
    {
        return startup.GetModule(enabled, infraAsCode);
    }
    
    public static MessagingModule GetMessaging<TInfra>(
        this IHostApplicationBuilder startup,
        TInfra infraAsCode)
        where TInfra : IArgumentProvider<InfrastructureAsCodeEnabled>, IInfrastructureAsCode<MessagingModule>
    {
        return startup.GetModule(infraAsCode.GetValue(), infraAsCode);
    }
}