using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;

namespace Momolith.Modules;

public static class HostApplicationBuilderExtensions
{
    public static TModule AddModule<TModule>(this IHostApplicationBuilder builder, Func<ModuleSetup, TModule> setup)
        where TModule : Module
    {
        return new HostApplicationBuilderModules(builder).AddModule(setup);
    }

    public static Task<TModule> AddModule<TModule>(this IHostApplicationBuilder builder, Func<ModuleSetup, Task<TModule>> setup)
        where TModule : Module
    {
        return new HostApplicationBuilderModules(builder).AddModule(setup);
    }
}

internal sealed record HostApplicationBuilderModules(IHostApplicationBuilder Builder) : IHostBuilderModules
{
    public IConfigurationManager Configuration => Builder.Configuration;

    public IServiceCollection Services => Builder.Services;
}