using Explicit.Configuration;
using Explicit.Validation;
using Microsoft.Extensions.Hosting;
using Momolith.ConfigurationAsCode;

namespace Momolith.Modules.Configuration;

public sealed class ConfigurationModule : IModule
{
    private readonly IHostApplicationBuilder _startup;
    private readonly ConfigurationAsCodeEnabled _configurationAsCode;
    private readonly IConfigurationModifier _modifier;

    private ConfigurationModule(
        IHostApplicationBuilder startup,
        ConfigurationAsCodeEnabled configurationAsCode,
        IConfigurationModifier modifier)
    {
        _startup = startup;
        _configurationAsCode = configurationAsCode;
        _modifier = modifier;
        _modifier.DownloadConfiguration(_startup.Configuration);
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
        _modifier.TryUpload(_configurationAsCode, configAsCode);
        
        _startup.Services.AddConfig<TOptions>(_startup.Configuration);
    }

    public IsValid<TOptions> AddGetConfig<TOptions>(IConfigurationAsCode<TOptions> configAsCode)
        where TOptions : IConfigObject<TOptions>
    {
        AddConfig(configAsCode);
        return _startup.Configuration.GetValid<TOptions>();
    }
}