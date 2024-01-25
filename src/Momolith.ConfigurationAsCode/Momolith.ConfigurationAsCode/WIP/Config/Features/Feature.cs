using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using OneOf;
using OneOf.Types;
using FluentValidation;
using Shared.Utils.Validation;
using Shared.Utils.Primitives;
using Microsoft.FeatureManagement.FeatureFilters;
using System.Text.Json;

namespace Shared.Configuration;

public static partial class Config
{
    public sealed class Feature<T> : ConfigurationAsCode.IRegistrable, IConfigElement
        where T : Feature.IName
    {
        public static string SectionName => $"FeatureManagement:{T.FeatureName}";

        public static void Register(ConfigurationModule module)
        {
            module.Services.AddFeatureManagement();
            module.Services.AddSingleton<Get>(provider => () => Create(provider.GetRequiredService<IFeatureManager>()));
        }

        public delegate Task<IsValid<ValueOf<bool, Feature.IsToggle>>> Get();

        public static async Task<IsValid<ValueOf<bool, Feature.IsToggle>>> Create(IFeatureManager manager)
        {
            var featureName = SectionName;
            var exists = manager.GetFeatureNamesAsync().ToBlockingEnumerable().Any(x => x == featureName);
            if(exists is false)
            {
                return new ValidationErrors($"Missing FeatureToggle :: [{featureName}]").AsIsValid<ValueOf<bool, Feature.IsToggle>>();
            }

            var isEnabled = await manager.IsEnabledAsync(featureName);

            return new ValueOf<bool, Feature.IsToggle>(isEnabled).IsValid();
        }

        public static OneOf<Success, ValidationErrors> ValidateConfig(IConfiguration configuration)
        {
            var featureName = Config.Feature<T>.SectionName;
            var section = configuration.GetSection(featureName);

            if(section is null)
            {
                return new ValidationErrors($"Missing FeatureToggle :: [{featureName}]");
            }

            return new Success();
        }
    }

    public static bool IsEnabled(this IsValid<ValueOf<bool, Feature.IsToggle>> isValid)
    {
        return isValid.True();
    }

    public static bool True<T>(this IsValid<T> isValid)
        where T : IValueOf<bool>
    {
        return isValid.IsT0 && isValid.AsT0.ValidValue.Value;
    }

    public static bool False<T>(this IsValid<T> isValid)
        where T : IValueOf<bool>
    {
        return isValid.True()!;
    }
}