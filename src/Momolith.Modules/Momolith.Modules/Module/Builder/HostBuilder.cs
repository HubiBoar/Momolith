using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Momolith.Modules;

public static class HostBuilderExtensions
{
    public static IHostBuilder SetupModules(this IHostBuilder builder, Action<IHostBuilderModules> modules)
    {
        Setup((config) => builder.ConfigureAppConfiguration(config), services => builder.ConfigureServices(services), modules);

        return builder;
    }

    public static IWebHostBuilder SetupModules(this IWebHostBuilder builder, Action<IHostBuilderModules> modules)
    {
        Setup((config) => builder.ConfigureAppConfiguration(config), services => builder.ConfigureServices(services), modules);

        return builder;
    }

    private static void Setup(
        Action<Action<IConfigurationBuilder>> configureAppConfiguration,
        Action<Action<IServiceCollection>> configureServices,
        Action<IHostBuilderModules> modules)
    {
        var _services = new ServiceCollection();

        configureAppConfiguration(config => {
            var configurationManager = new InternalConfigurationManager(config);
            var hostBuilderModules = new HostBuilderModules(configurationManager, _services);
            modules(hostBuilderModules);
        });

        configureServices(services => {
            foreach(var service in _services)
            {
                services.Add(service);
            }
        });
    }
}