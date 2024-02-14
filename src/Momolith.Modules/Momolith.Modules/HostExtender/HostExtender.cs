using Microsoft.Extensions.Hosting;

namespace Momolith.Modules;

public interface IHostExtender<THost>
    where THost : IHost
{
    void Extend(Action<THost> endpoint);

    void ExtendAsync(Func<THost, Task> endpoint);

    Task RunExtensionsAsync(THost host);
}

public interface IHostExtender : IHostExtender<IHost>
{
}

public sealed class HostExtender : IHostExtender
{
    private readonly List<Action<IHost>> _extensions = new ();
    private readonly List<Func<IHost, Task>> _extensionsAsync = new ();

    public void Extend(Action<IHost> endpoint)
    {
        _extensions.Add(endpoint);
    }

    public void ExtendAsync(Func<IHost, Task> endpoint)
    {
        _extensionsAsync.Add(endpoint);
    }

    public async Task RunExtensionsAsync(IHost host)
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

public static class HostExtensions
{
    public static HostExtender AddExtender(this HostApplicationBuilder builder)
    {
        return new HostExtender();
    }

    public static HostExtender AddExtender(this IHostBuilder builder)
    {
        return new HostExtender();
    }

    public static Task ExtendHost<THost>(this THost host, IHostExtender<THost> hostExtender)
        where THost : IHost
    {
        return hostExtender.RunExtensionsAsync(host);
    }
}