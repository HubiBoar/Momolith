using Explicit.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FeatureSlice;

public interface IFeatureSlicePipeline<TRequest, TResponse> : IFeatureSliceBase
{
    public Task<TResponse> Handle(TRequest request, Func<TRequest, TResponse> next);

    static ServiceLifetime IFeatureSliceBase.ServiceLifetime => ServiceLifetime.Singleton;

    static string IFeatureSliceBase.FeatureName => typeof(IFeatureSlicePipeline<TRequest, TResponse>).GetTypeVerboseName();

    static void IFeatureSliceBase.Register<T>(IServiceCollection collection)
    {
        collection.TryAdd(new ServiceDescriptor(typeof(IFeatureSlicePipeline<TRequest, TResponse>), typeof(T), T.ServiceLifetime));
    }
}