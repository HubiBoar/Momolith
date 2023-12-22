using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FeatureSlice;

public interface IFeatureSliceBase
{
    public static abstract string FeatureName { get; }

    public static abstract ServiceLifetime ServiceLifetime { get; }
}

public interface IFeatureSlice<TRequest, TResponse> : IFeatureSliceBase
{
    static ServiceLifetime IFeatureSliceBase.ServiceLifetime => ServiceLifetime.Transient;

    public IFeatureSliceDispatcher<TRequest, TResponse> Dispatcher { get; }

    protected Task<TResponse> Handle(TRequest request);
}