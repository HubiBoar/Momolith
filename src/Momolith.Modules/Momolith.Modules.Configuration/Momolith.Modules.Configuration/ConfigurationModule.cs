using Explicit.Configuration;
using Explicit.Utils;
using Explicit.Validation;
using Microsoft.Extensions.Hosting;
using Momolith.ConfigurationAsCode;

namespace Momolith.Modules.Configuration;

public sealed class ConfigurationModule : IModule
{
    private IHostApplicationBuilder Startup { get; }
    private IConfigurationModifier Modifier { get; }
    private ConfigurationAsCodeEnabled ConfigurationAsCode { get; }

    private ConfigurationModule(
        IHostApplicationBuilder startup,
        ConfigurationAsCodeEnabled configurationAsCode,
        IConfigurationModifier modifier)
    {
        Startup = startup;
        Modifier = modifier;
        Modifier.DownloadConfiguration(Startup.Configuration);

        ConfigurationAsCode = configurationAsCode;
    }

    public static ConfigurationModule Create(
        IHostApplicationBuilder startup,
        ConfigurationAsCodeEnabled enabled,
        IConfigurationModifier modifier)
    {
        return new ConfigurationModule(startup, enabled, modifier);
    }

    public void AddConfig<TOptions>(IConfigurationAsCode<TOptions> configAsCode)
        where TOptions : IConfigObject<TOptions>
    {
        Modifier.TryUpload(ConfigurationAsCode, configAsCode);
        
        Startup.Services.AddConfig<TOptions>(Startup.Configuration);
    }

    public IsValid<TOptions> AddGetConfig<TOptions>(IConfigurationAsCode<TOptions> configAsCode)
        where TOptions : IConfigObject<TOptions>
    {
        AddConfig(configAsCode);
        return Startup.Configuration.GetValid<TOptions>();
    }
}