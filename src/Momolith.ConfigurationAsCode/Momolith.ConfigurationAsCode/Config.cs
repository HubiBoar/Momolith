using Explicit.Configuration;

namespace Momolith.ConfigurationAsCode;

public sealed class SecretConfig<TOptions>
    where TOptions : IConfigObject<TOptions>
{
    public string SecretName { get; }
    
    internal SecretConfig(string secretName)
    {
        SecretName = secretName;
    }
}

public sealed class ManualConfig<TOptions>
    where TOptions : IConfigObject<TOptions>
{
    internal ManualConfig()
    {
    }
}
