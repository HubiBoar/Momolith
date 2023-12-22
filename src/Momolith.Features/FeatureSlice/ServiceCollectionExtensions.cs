using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using OneOf;

namespace FeatureSlice;

public static class ServiceCollectionExtensions
{
    public static void AddFeature<TService, TImplementation>(this IServiceCollection services)
        where TService : class, IFeatureSliceBase
        where TImplementation : class, TService
    {
        services.AddFeatureManagement();
        
        TService.RegisterDispatcher(services);
        services.TryAddTransient<TService, TImplementation>();
    }

    public static string GetFeatureName<TFeature>(this TFeature feature)
        where TFeature : IFeatureSliceBase
    {
        return TFeature.FeatureName;
    }
}