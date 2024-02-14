using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Momolith.Modules;

internal sealed class HostBuilderModules : IHostBuilderModules
{
    public IConfigurationManager Configuration { get; }
    public IServiceCollection Services { get; }

    public HostBuilderModules(IConfigurationBuilder configurationBuilder, IServiceCollection services)
    {
        Configuration = new InternalConfigurationManager(configurationBuilder);
        Services = services;
    }
}

internal sealed record InternalConfigurationManager(IConfigurationBuilder Builder) : IConfigurationManager
{
    public string? this[string key] { get => Configuration[key]; set => Configuration[key] = value; }

    public IConfigurationRoot Configuration => Build();

    public IDictionary<string, object> Properties => Builder.Properties;

    public IList<IConfigurationSource> Sources => Builder.Sources;

    public IConfigurationBuilder Add(IConfigurationSource source)
    {
        return Builder.Add(source);
    }

    public IConfigurationRoot Build()
    {
        return Builder.Build();
    }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        return Configuration.GetChildren();
    }

    public IChangeToken GetReloadToken()
    {
        return Configuration.GetReloadToken();
    }

    public IConfigurationSection GetSection(string key)
    {
        return Configuration.GetSection(key);
    }
}