using Microsoft.Extensions.Hosting;
using Momolith.ConfigurationAsCode;

namespace Momolith.Modules.Configuration.Default;

public sealed class InMemoryConfigurationProvider
{
    public static ConfigurationModule GetConfiguration(IHostApplicationBuilder startup, ConfigurationAsCodeEnabled enabled)
    {
        var modifier = new InMemoryConfigurationModifier(startup.Configuration);
        return ConfigurationModule.Create(startup, enabled, modifier);
    }
}