using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Momolith.Modules.Web;

public interface IEndpoint
{
    public static abstract IEndpointConventionBuilder Map(IEndpointRouteBuilder endpoint);
}

public static class EndpointExtensions
{
    public static IEndpointConventionBuilder MapEndpoint<T>(this IEndpointRouteBuilder endpoint)
        where T : IEndpoint
    {
        return T.Map(endpoint);
    }
}