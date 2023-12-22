using Microsoft.Extensions.DependencyInjection;

namespace FeatureSlice;

public class ExampleRequest
{
}

public class ExampleResponse
{
}

public interface IFeatureExample : IFeatureSlice<ExampleRequest, ExampleResponse>
{
}

internal sealed class FeatureExample : IFeatureExample
{
    public static string FeatureName { get; } = "FeatureExample";

    public IFeatureSliceDispatcher<ExampleRequest, ExampleResponse> Dispatcher { get; }
    
    public FeatureExample(IFeatureSliceDispatcher<ExampleRequest, ExampleResponse> dispatcher)
    {
        Dispatcher = dispatcher;
    }

    public Task<ExampleResponse> Handle(ExampleRequest request)
    {
        
    }
}

public class RegistrationExample
{
    public void Register(IServiceCollection collection)
    {
        collection.AddFeature<FeatureExample>();
    }
}