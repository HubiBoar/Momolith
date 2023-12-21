using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Momolith.Modules;

public static class ModularStartupExtensions
{
    public static T AddModule<T>(this IHostApplicationBuilder builder, Func<ModuleSetup, T> factory)
        where T : class, IModule
    {
        var setup = new ModuleSetup(builder.Services, builder.Configuration);

        var module = factory(setup);
        
        builder.Services.AddSingleton(module);

        return module;
    }
    
    public static void MapModules<THost>(this THost host)
        where THost : IHost
    {
        var services = host.Services.GetServices<HostOptions<THost>>();
        foreach (var service in services)
        {
            service.Run(host);
        }
        
        var genericServices = host.Services.GetServices<HostOptions<IHost>>();
        foreach (var service in genericServices)
        {
            service.Run(host);
        }
    }
}

public static class ModularStartupHelper<TBuilder, THost>
    where TBuilder : IHostApplicationBuilder
    where THost : IHost
{
    public static T AddModule<T>(TBuilder builder, Func<ModuleSetup, HostOptions<THost>, T> factory)
        where T : class, IModule
    {
        var setup = new ModuleSetup(builder.Services, builder.Configuration);

        var hostSetup = new HostOptions<THost>();

        builder.Services.AddSingleton(hostSetup);

        var module = factory(setup, hostSetup);

        builder.Services.AddSingleton(module);

        return module;
    }
}