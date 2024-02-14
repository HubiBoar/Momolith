using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace Momolith.Modules;

public sealed class WebAppExtender : IHostExtender<WebApplication>, IHostExtender
{
    private readonly List<Action<IHost>> _extensions = new ();
    private readonly List<Func<IHost, Task>> _extensionsAsync = new ();

    private readonly List<Action<WebApplication>> _webExtensions = new ();
    private readonly List<Func<WebApplication, Task>> _webExtensionsAsync = new ();

    public void Extend(Action<IHost> endpoint)
    {
        _extensions.Add(endpoint);
    }

    public void ExtendAsync(Func<IHost, Task> endpoint)
    {
        _extensionsAsync.Add(endpoint);
    }

    public void Extend(Action<WebApplication> endpoint)
    {
        _webExtensions.Add(endpoint);
    }

    public void ExtendAsync(Func<WebApplication, Task> endpoint)
    {
        _webExtensionsAsync.Add(endpoint);
    }

    public void Map(Action<IEndpointRouteBuilder> endpoint)
    {
        Extend(host => endpoint(host));
    }
    
    public async Task RunExtensionsAsync(WebApplication host)
    {
        await RunExtensionsAsyncHost(host);
        foreach(var extension in _webExtensions)
        {
            extension(host);
        }

        foreach(var extension in _webExtensionsAsync)
        {
           await extension(host);
        }
    }

    public Task RunExtensionsAsync(IHost host)
    {
        return RunExtensionsAsyncHost(host);
    }

    private async Task RunExtensionsAsyncHost(IHost host)
    {
        foreach(var extension in _extensions)
        {
            extension(host);
        }

        foreach(var extension in _extensionsAsync)
        {
           await extension(host);
        }
    }
}

public static class WebExtensionExtensions
{
    public static WebAppExtender AddExtender(this WebApplicationBuilder host)
    {
        return new WebAppExtender();
    }
}