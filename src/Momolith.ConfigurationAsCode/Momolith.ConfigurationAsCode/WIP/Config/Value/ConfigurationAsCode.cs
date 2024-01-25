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

public static class ValueConfigAsCodeExtensions
{
    public static ConfigurationAsCode.Reference<TSelf> Value<TSelf, TValue>(this ConfigurationAsCode.Context<TSelf> context, TValue value)
        where TSelf : Config.IValue<TValue>, IConfigElement
    {
        var json = JsonSerializer.Serialize(value);
        return new ConfigurationAsCode.Reference<TSelf>(TSelf.SectionName, json, TSelf.ValidateConfig);
    }
}