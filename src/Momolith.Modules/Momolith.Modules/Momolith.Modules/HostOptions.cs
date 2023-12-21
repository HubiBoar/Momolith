using Microsoft.Extensions.Hosting;

namespace Momolith.Modules;

public class HostOptions<THost> where THost : IHost
{
    private Action<THost>? Setup { get; set; }

    internal void Run(THost host)
    {
        Setup?.Invoke(host);
    }
    
    public void OnHost(Action<THost> setup)
    {
        Setup = setup;
    }
}