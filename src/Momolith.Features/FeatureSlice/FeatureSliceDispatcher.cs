using Microsoft.FeatureManagement;
using OneOf;

namespace FeatureSlice;

public sealed record Disabled;

public interface IFeatureSliceDispatcher<TRequest, TResponse>
{
    internal Task<OneOf<TResponse, Disabled>> Send<TFeature>(TFeature feature, TRequest request)
        where TFeature : IFeatureSlice<TRequest, TResponse>;
}

internal sealed class FeatureSliceDispatcher<TRequest, TResponse> : IFeatureSliceDispatcher<TRequest, TResponse>
{
    private readonly IFeatureManager _featureManager;
    private readonly IEnumerable<IFeatureSlicePipeline<TRequest, TResponse>> _pipelines;

    public FeatureSliceDispatcher(
        IFeatureManager featureManager,
        IEnumerable<IFeatureSlicePipeline<TRequest, TResponse>> pipelines)
    {
        _featureManager = featureManager;
        _pipelines = pipelines;
    }
    
    public async Task<OneOf<TResponse, Disabled>> Send<TFeature>(TFeature feature, TRequest request)
        where TFeature : IFeatureSlice<TRequest, TResponse>
    {
        var enabled = await _featureManager.IsEnabledAsync(TFeature.FeatureName);

        if (enabled == false)
        {
            return new Disabled();
        }

        foreach (var pipeline in _pipelines)
        {
            
        }
    }
}