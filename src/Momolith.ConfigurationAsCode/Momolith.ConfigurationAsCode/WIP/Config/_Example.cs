using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using OneOf;
using OneOf.Types;
using Shared.Startup;
using Shared.Utils.Validation;

namespace Shared.Configuration;

internal static class Example
{
    private sealed class Section : Config.Section<Section>, Config.ISection<Section>
    {
        public static string SectionName => "ExampleSection";

        public required string Value1 { get; set; } = string.Empty;
        public required string Value2 { get; set; } = string.Empty;

        public static void SetupValidation(FluentValidator<Section> validator)
        {
            validator.RuleFor(x => x.Value1).NotEmpty();
            validator.RuleFor(x => x.Value2).NotEmpty();
        }
    }

    private sealed class Feature : Config.Feature.IName
    {
        public static string FeatureName => "ExampleFeature";
    }

    private sealed record Value : Config.Value<Value, string, IsConnectionString>, Config.IValue
    {
        public static string SectionName => "ExampleValue";
    }

    private static async Task Run(
        Section.Get exampleSection,
        Value.Get exampleValue,
        Config.Feature<Feature>.Get featureToggle)
    {
        if(exampleSection().IsValid(out var section))
        {
            var val1 = section.ValidValue.Value1;
        }

        if(exampleValue().IsValid(out var value))
        {
        }

        var toggle = await featureToggle();

        if(toggle.IsEnabled())
        { 
        }
    }

    private static void Register(ConfigurationModule module, Environment environment)
    {
        module.AddConfig<Section>(environment);
        module.AddConfig<Value>(environment);
        module.AddConfig<Config.Feature<Feature>>(environment);
    }

    private sealed partial class Environment : EnvironmentBase
    {
        public Environment(IConfiguration configuration) : base(configuration)
        {
        }
    }

    private sealed partial class Environment :
        ConfigurationAsCode.IProvider<Section>,
        ConfigurationAsCode.IProvider<Value>,
        ConfigurationAsCode.IProvider<Config.Feature<Feature>>
    {
        public ConfigurationAsCode.Reference<Section> ConfigurationAsCode(ConfigurationAsCode.Context<Section> context)
        {
            return Match<ConfigurationAsCode.Reference<Section>>(
                prod =>  context.Value(
                    new Section()
                    {
                        Value1 = "prodVal1",
                        Value2 = default!
                    })
                    .ManualParam(x => x.Value1)
                    .SecretParam(x => x.Value2, "Section-Value2Secret"),
                acc => context.Secret("SectionSecret"),
                test => context.Value(
                    new Section()
                    {
                        Value1 = "testVal1",
                        Value2 = "testVal2"
                    })
            );
        }

        public ConfigurationAsCode.Reference<Value> ConfigurationAsCode(ConfigurationAsCode.Context<Value> context)
        { 
            return Match<ConfigurationAsCode.Reference<Value>>(
                prod => context.Secret("ValueSecret"),
                acc => context.Manual(),
                test => context.Value("testValue")
            );
        }

        public ConfigurationAsCode.Reference<Config.Feature<Feature>> ConfigurationAsCode(ConfigurationAsCode.Context<Config.Feature<Feature>> context)
        {
            return Match<ConfigurationAsCode.Reference<Config.Feature<Feature>>>(
                prod => context.Filters(
                    new Config.Feature.Filters.Percentage(50),
                    new Config.Feature.Filters.TimeWindow(
                        DateTimeOffset.Now,
                        DateTimeOffset.Now.AddDays(5))),
                acc => context.Value(true),
                test => context.Value(true)
            );
        }
    }
}
