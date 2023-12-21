using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Momolith.Modules;

public class ModuleSetup
{
    public IServiceCollection Services { get; }

    public IConfiguration Configuration { get; }

    //TODO StartupLogger
    //public StartupLogger Logger { get; }

    internal ModuleSetup(IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
    }
}