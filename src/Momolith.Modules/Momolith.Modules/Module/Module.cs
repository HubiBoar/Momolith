using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Momolith.Modules;

public sealed record ModuleSetup(IServiceCollection Services, IConfigurationManager Configuration)
{
}

public abstract class Module
{
    public ModuleSetup Setup { get; }

    public IServiceCollection Services => Setup.Services;

    public IConfigurationManager Configuration => Setup.Configuration;

    protected Module(ModuleSetup setup)
    {
        Setup = setup;
    }
}

public interface IHostBuilderModules
{
    public IConfigurationManager Configuration { get; }

    public IServiceCollection Services { get; }
}

public static class HostBuilderModulesExtensions
{
    public static TModule AddModule<TModule>(this IHostBuilderModules modules, Func<ModuleSetup, TModule> setup)
        where TModule : Module
    {
        return setup(new ModuleSetup(modules.Services, modules.Configuration));
    }

    public static Task<TModule> AddModule<TModule>(this IHostBuilderModules modules, Func<ModuleSetup, Task<TModule>> setup)
        where TModule : Module
    {
        return setup(new ModuleSetup(modules.Services, modules.Configuration));
    }
}