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
    public static partial class Feature
    {
        public interface IName
        {
            public static abstract string FeatureName { get; }
        }

        public sealed class IsToggle : IValidateRule<bool>
        {
            public static void SetupValidation<T>(IRuleBuilder<T, bool> ruleBuilder)
            {
                ruleBuilder.NotNull();
            }
        }
    }
}