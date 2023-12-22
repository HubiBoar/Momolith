using Microsoft.Extensions.DependencyInjection;
using OneOf;

namespace FeatureSlice;

public interface IFeatureSliceBase
{
    public static abstract string FeatureName { get; }

    public static abstract void RegisterDispatcher(IServiceCollection collection);
}

public interface IFeatureSlice<TRequest, TResponse> : IFeatureSliceBase
{
    public IDispatcher<TRequest, TResponse> Dispatcher { get; }

    protected Task<TResponse> Handle(TRequest request);

    static void IFeatureSliceBase.RegisterDispatcher(IServiceCollection collection)
    {
        collection.AddSingleton<IDispatcher<TRequest, TResponse>, FeatureSliceDispatcher<TRequest, TResponse>>();
    }
    
    public async Task<OneOf<TResponse, Disabled, Exception>> Send(TRequest request)
    {
        try
        {
            var response = await Dispatcher.Send(this, request);

            return response.Match<OneOf<TResponse, Disabled, Exception>>(r => r, d => d);
        }
        catch (Exception exception)
        {
            return exception;
        } 
    }
}