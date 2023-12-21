using Microsoft.AspNetCore.Builder;

namespace Momolith.Modules.Web;

public static class StartupExtensions
{
    public static T AddModule<T>(this WebApplicationBuilder builder, Func<ModuleSetup, HostOptions<WebApplication>, T> factory)
        where T : class, IModule
    {
        return ModularStartupHelper<WebApplicationBuilder, WebApplication>.AddModule(builder, factory);
    }
}