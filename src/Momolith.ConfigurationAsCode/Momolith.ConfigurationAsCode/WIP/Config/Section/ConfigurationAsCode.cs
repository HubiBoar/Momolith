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

public static class SectionConfigAsCodeExtensions
{
    public static ConfigurationAsCode.Reference<TSelf> Value<TSelf>(this ConfigurationAsCode.Context<TSelf> context, TSelf value)
        where TSelf : Config.Section<TSelf>, Config.ISection<TSelf>
    {
        var json = JsonSerializer.Serialize(value);

        return new ConfigurationAsCode.Reference<TSelf>(TSelf.SectionName, json, Config.Section<TSelf>.ValidateConfig);
    }

    public static ConfigurationAsCode.Reference<TSelf> ManualParam<TSelf, TValue>(this ConfigurationAsCode.Reference<TSelf> reference, Expression<Func<TSelf, TValue>> expression)
        where TSelf : Config.Section<TSelf>, Config.ISection<TSelf>
    {
        var sectionName = SectionName(TSelf.SectionName, expression);

        reference.Results.Add(sectionName, new ConfigurationAsCode.Manual());

        return reference;
    }

    public static ConfigurationAsCode.Reference<TSelf> SecretParam<TSelf, TValue>(this ConfigurationAsCode.Reference<TSelf> reference, Expression<Func<TSelf, TValue>> expression, string secretName)
        where TSelf : Config.Section<TSelf>, Config.ISection<TSelf>
    {
        var sectionName = SectionName(TSelf.SectionName, expression);

        reference.Results.Add(sectionName, new ConfigurationAsCode.Secret(secretName));

        return reference;
    }

    private static string SectionName<T>(string sectionName, Expression<T> expression)
    {
        var member = expression.Body as MemberExpression;
        var parameterName = member!.Member.Name;

        return $"{sectionName}:{parameterName}";
    }
}