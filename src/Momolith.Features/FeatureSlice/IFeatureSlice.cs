using Microsoft.Extensions.DependencyInjection;
using OneOf;

namespace FeatureSlice;

public interface IFeatureName
{
    public static abstract string FeatureName { get; }
}

public interface IFeatureSliceBase : IFeatureName
{
    public static abstract void RegisterDispatcher<T>(IServiceCollection collection)
        where T : IFeatureSliceBase;
}

public interface IFeatureSlice<TRequest, TResponse> : IFeatureSliceBase
{
    public IDispatcher<TRequest, TResponse> Dispatcher { get; }

    protected Task<TResponse> Handle(TRequest request);

    static void IFeatureSliceBase.RegisterDispatcher<T>(IServiceCollection collection)
    {
        collection.AddSingleton<IDispatcher<TRequest, TResponse>, FeatureSliceDispatcher<TRequest, TResponse, T>>();
    }
    
    public async Task<OneOf<TResponse, Disabled, Exception>> Send(TRequest request)
    {
        try
        {
            var response = await Dispatcher.Send(Handle, request);

            return response.Match<OneOf<TResponse, Disabled, Exception>>(r => r, d => d);
        }
        catch (Exception exception)
        {
            return exception;
        } 
    }
}