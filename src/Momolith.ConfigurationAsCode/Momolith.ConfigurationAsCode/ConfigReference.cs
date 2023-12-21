using Explicit.Configuration;
using OneOf;

namespace Momolith.ConfigurationAsCode;

public sealed record ConfigReference<TOptions>
    where TOptions : IConfigObject<TOptions>
{
    public OneOf<TOptions, ManualConfig<TOptions>, SecretConfig<TOptions>> Result { get; }

    internal ConfigReference(TOptions options)
    {
        Result = options;
    }

    public ConfigReference(SecretConfig<TOptions> secretConfig)
    {
        Result = secretConfig;
    }
    
    public ConfigReference(ManualConfig<TOptions> manualConfig)
    {
        Result = manualConfig;
    }
    
    public static implicit operator ConfigReference<TOptions>(TOptions options)
    {
        return new ConfigReference<TOptions>(options);
    }

    public static implicit operator ConfigReference<TOptions>(SecretConfig<TOptions> secretConfig)
    {
        return new ConfigReference<TOptions>(secretConfig);
    }
    
    public static implicit operator ConfigReference<TOptions>(ManualConfig<TOptions> manualConfig)
    {
        return new ConfigReference<TOptions>(manualConfig);
    }
}