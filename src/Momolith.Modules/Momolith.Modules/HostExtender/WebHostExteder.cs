using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Momolith.Modules;

public class WebHost : IHost
{
    public IServiceProvider Services => Host.Services;

    public IWebHost Host { get; }

    public WebHost(IWebHost webHost)
    {
        Host = webHost;
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return Host.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Host.StartAsync(cancellationToken);
    }

    public void Dispose()
    {
        Host.Dispose();
    }
}

public sealed class WebHostExtender : IHostExtender<WebHost>, IHostExtender
{
    private readonly List<Action<IHost>> _extensions = new ();
    private readonly List<Func<IHost, Task>> _extensionsAsync = new ();

    private readonly List<Action<WebHost>> _webExtensions = new ();
    private readonly List<Func<WebHost, Task>> _webExtensionsAsync = new ();

    public void Extend(Action<IHost> endpoint)
    {
        _extensions.Add(endpoint);
    }

    public void ExtendAsync(Func<IHost, Task> endpoint)
    {
        _extensionsAsync.Add(endpoint);
    }

    public void Extend(Action<WebHost> endpoint)
    {
        _webExtensions.Add(endpoint);
    }

    public void ExtendAsync(Func<WebHost, Task> endpoint)
    {
        _webExtensionsAsync.Add(endpoint);
    }

    public Task RunExtensionsAsync(IWebHost host)
    {
        return RunExtensionsAsync(host);
    }
    
    public async Task RunExtensionsAsync(WebHost host)
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

public static class WebHostExtensionExtensions
{
    public static WebHostExtender AddHostExtender(this IWebHostBuilder host)
    {
        return new WebHostExtender();
    }

    public static Task ExtendHost(this IWebHost host, WebHostExtender hostExtender)
    {
        return hostExtender.RunExtensionsAsync(host);
    }
}