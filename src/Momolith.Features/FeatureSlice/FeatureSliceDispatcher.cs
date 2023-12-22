using Microsoft.FeatureManagement;
using OneOf;

namespace FeatureSlice;

public sealed record Disabled;

public interface IDispatcher<TRequest, TResponse>
{
    internal Task<OneOf<TResponse, Disabled>> Send(IFeatureSlice<TRequest, TResponse> featureSlice, TRequest request);
}

internal sealed class FeatureSliceDispatcher<TRequest, TResponse>
    : IDispatcher<TRequest, TResponse>
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
    
    public async Task<OneOf<TResponse, Disabled>> Send(IFeatureSlice<TRequest, TResponse> featureSlice, TRequest request)
    {
        var enabled = await _featureManager.IsEnabledAsync(featureSlice.FeatureName);

        if (enabled == false)
        {
            return new Disabled();
        }

        foreach (var pipeline in _pipelines)
        {
            
        }
    }
}