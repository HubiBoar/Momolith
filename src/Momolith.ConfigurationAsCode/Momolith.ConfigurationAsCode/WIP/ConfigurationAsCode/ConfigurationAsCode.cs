using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using OneOf;
using OneOf.Types;
using Shared.Configuration;
using Shared.Utils.Validation;

namespace Shared.Configuration;

public static partial class ConfigurationAsCode
{
    public interface IRegistrable
    {
        public abstract static OneOf<Success, ValidationErrors> ValidateConfig(IConfiguration configuration);

        public abstract static void Register(ConfigurationModule module);
    }

    public interface IProvider<TConfig>
        where TConfig : IConfigElement
    {
        public Reference<TConfig> ConfigurationAsCode(Context<TConfig> context);
    }

    public sealed class Context<TConfig>
        where TConfig : IConfigElement
    {
        internal Context()
        {
        }
    }


    public static void AddConfig<T>(this ConfigurationModule module, ConfigurationAsCode.IProvider<T> provider)
        where T : IRegistrable, IConfigElement
    {
        T.Register(module);
    }
}

public static class ConfigurationAsCodeExtensions
{
    public static ConfigurationAsCode.Reference<TSelf> Secret<TSelf>(this ConfigurationAsCode.Context<TSelf> context, string secretName)
        where TSelf : ConfigurationAsCode.IRegistrable, IConfigElement
    {
        return new ConfigurationAsCode.Reference<TSelf>(TSelf.SectionName, new ConfigurationAsCode.Secret(secretName), TSelf.ValidateConfig);
    }

    public static ConfigurationAsCode.Reference<TSelf> Manual<TSelf>(this ConfigurationAsCode.Context<TSelf> context)
        where TSelf : ConfigurationAsCode.IRegistrable, IConfigElement
    {
        return new ConfigurationAsCode.Reference<TSelf>(TSelf.SectionName, new ConfigurationAsCode.Manual(), TSelf.ValidateConfig);
    }
}