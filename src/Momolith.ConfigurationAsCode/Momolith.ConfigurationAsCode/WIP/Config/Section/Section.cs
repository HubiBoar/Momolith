using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using OneOf;
using OneOf.Types;
using Shared.Utils.Validation;

namespace Shared.Configuration;

public static partial class Config
{
    public interface ISection<TSelf> : IConfigElement, IValidate<TSelf>
        where TSelf : Section<TSelf>, ISection<TSelf>
    {
    }

    public abstract class Section<TSelf> : ConfigurationAsCode.IRegistrable, IGetConfig<IsValid<TSelf>>
        where TSelf : Section<TSelf>, ISection<TSelf>
    {
        public delegate IsValid<TSelf> Get();

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

        public static IsValid<TSelf> FromConfig(IConfiguration configuration)
        {
            var sectionName = TSelf.SectionName;
            var section = configuration.GetSection(sectionName).Get<TSelf>();

            if(section is null)
            {
                return new ValidationErrors($"Missing Section :: [{sectionName}]").AsIsValid<TSelf>();
            }

            return section.IsValid();
        }
    }
}