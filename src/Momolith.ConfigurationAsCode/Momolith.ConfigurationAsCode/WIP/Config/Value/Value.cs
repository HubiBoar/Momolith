using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using OneOf;
using OneOf.Types;
using Shared.Utils.Primitives;
using Shared.Utils.Validation;

namespace Shared.Configuration;

public static partial class Config
{
    public interface IValue<TValue> : ConfigurationAsCode.IRegistrable
    {
    }

    public interface IValue : IConfigElement
    {
    }

    public abstract record Value<TSelf, TValue, TMethod> : IValue<TValue>, IGetConfig<IsValid<ValueOf<TValue, TMethod>>>
        where TSelf : Value<TSelf, TValue, TMethod>, IValue
        where TMethod : IValidateRule<TValue>
    {
        public delegate IsValid<ValueOf<TValue, TMethod>> Get();

        public static void Register(ConfigurationModule module)
        {
            var configKey = ConfigKey;
            module.Services.TryAddKeyedSingleton(configKey, module.Configuration);

            module.Services.AddSingleton<Get>(provider => () => FromConfig(provider.GetRequiredKeyedService<IConfiguration>(configKey)));
        }

        public static OneOf<Success, ValidationErrors> ValidateConfig(IConfiguration configuration)
        {
            return FromConfig(configuration).AsSuccess;
        }

        public static IsValid<ValueOf<TValue, TMethod>> FromConfig(IConfiguration configuration)
        {
            var sectionName = TSelf.SectionName;
            var value = configuration.GetSection(sectionName).Get<TValue>();

            if(value is null)
            {
                return new ValidationErrors($"Missing Section for Value :: [{sectionName}]").AsIsValid<ValueOf<TValue, TMethod>>();
            }

            return new ValueOf<TValue, TMethod>(value).IsValid();
        }
    }
}