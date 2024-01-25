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
        public static class Filters
        {
            public interface IFilter
            {
                public string Name { get; }
            }

            public interface IFilter<T> : IFilter
            {
                public T Parameters { get; }
            }

            public sealed class Percentage : IFilter<PercentageFilterSettings>
            {
                public string Name => "Microsoft.Percentage";

                public PercentageFilterSettings Parameters { get; }

                public Percentage(int value)
                {
                    Parameters = new PercentageFilterSettings()
                    {
                        Value = value
                    };
                }
            }

            public sealed class TimeWindow : IFilter<TimeWindowFilterSettings>
            {
                public string Name => "Microsoft.TimeWindow";

                public TimeWindowFilterSettings Parameters { get; }

                public TimeWindow(DateTimeOffset? start, DateTimeOffset? end)
                {
                    Parameters = new TimeWindowFilterSettings()
                    {
                        Start = start,
                        End = end
                    };
                }
            }

            public sealed class Targeting : IFilter<TargetingFilterSettings>
            {
                public string Name => "Microsoft.Targeting";

                public TargetingFilterSettings Parameters { get; }

                public Targeting(Audience audience)
                {
                    Parameters = new TargetingFilterSettings()
                    {
                        Audience = audience
                    };
                }
            }
        }
    }
}