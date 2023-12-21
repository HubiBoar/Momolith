using Microsoft.Extensions.Hosting;

namespace Momolith.Modules.Messaging.Default;

public static class InMemoryMessagingProvider
{
    public static MessagingComponent GetMessaging(IHostApplicationBuilder startup)
    {
        return MessagingComponent.Create<InMemoryMessagingConfiguration>(startup);
    }
}