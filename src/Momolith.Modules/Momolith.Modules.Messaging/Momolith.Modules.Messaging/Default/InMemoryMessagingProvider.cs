using Microsoft.Extensions.Hosting;

namespace Momolith.Modules.Messaging.Default;

public static class InMemoryMessagingProvider
{
    public static MessagingModule GetMessaging(IHostApplicationBuilder startup)
    {
        return MessagingModule.Create<InMemoryMessagingConfiguration>(startup);
    }
}