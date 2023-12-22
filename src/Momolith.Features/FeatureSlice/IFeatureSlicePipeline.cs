namespace FeatureSlice;

public interface IFeatureSlicePipeline<TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest request, Func<TRequest, Task<TResponse>> next);
}