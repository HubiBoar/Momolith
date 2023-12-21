using Explicit.Configuration;

namespace Momolith.ConfigurationAsCode;

public interface IConfigurationAsCode<TOptions>
    where TOptions : IConfigObject<TOptions>
{
    public ConfigReference<TOptions> ConfigurationAsCode(ConfigurationAsCodeContext<TOptions> context);
}

public sealed class ConfigurationAsCodeContext<TOptions>
    where TOptions : IConfigObject<TOptions>
{
    public SecretConfig<TOptions> Secret { get; } = new (TOptions.SectionName);
    
    public SecretConfig<TOptions> SecretWithName(string secretName) => new (secretName);

    public ManualConfig<TOptions> Manual { get; } = new ();

    internal ConfigurationAsCodeContext()
    {
    }
}