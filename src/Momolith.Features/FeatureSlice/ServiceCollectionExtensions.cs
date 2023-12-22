using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using OneOf;

namespace FeatureSlice;

public static class ServiceCollectionExtensions
{
    public static void AddFeature<TService, TImplementation>(this IServiceCollection services)
        where TService : class, IFeatureSliceBase
        where TImplementation : TService
    {
        services.AddFeatureManagement();

        var service
        
        services.TryAdd<TService, TImplementation>();
    }

    public static string GetFeatureName<TFeature>(this TFeature feature)
        where TFeature : IFeatureSliceBase
    {
        return TFeature.FeatureName;
    }
    
    public static async Task<OneOf<TResponse, Disabled, Exception>> Send<TFeature, TRequest, TResponse>(this TFeature feature, TRequest request)
        where TFeature : class, IFeatureSlice<TRequest, TResponse>
    {
        try
        {
            var response = await feature.Dispatcher.Send<TFeature>(feature, request);

            return response;
        }
        catch (Exception exception)
        {
            return exception;
        } 
    }
}