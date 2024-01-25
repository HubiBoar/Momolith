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

public static class FeatureConfigAsCodeExtensions
{
    private sealed record FiltersEnabledFor(IReadOnlyCollection<Config.Feature.Filters.IFilter> EnabledFor);

    public static ConfigurationAsCode.Reference<Config.Feature<T>> Filters<T>(this ConfigurationAsCode.Context<Config.Feature<T>> context, params Config.Feature.Filters.IFilter[] filters)
        where T : Config.Feature.IName
    {
        var enabledFor = new FiltersEnabledFor(filters);
        var json = JsonSerializer.Serialize(enabledFor);

        return new ConfigurationAsCode.Reference<Config.Feature<T>>(Config.Feature<T>.SectionName, json, Config.Feature<T>.ValidateConfig);
    }
    public static ConfigurationAsCode.Reference<Config.Feature<T>> Value<T>(this ConfigurationAsCode.Context<Config.Feature<T>> context, bool value)
        where T : Config.Feature.IName
    {
        return new ConfigurationAsCode.Reference<Config.Feature<T>>(Config.Feature<T>.SectionName, value.ToString(), Config.Feature<T>.ValidateConfig);
    }
}