using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Momolith.Modules.Messaging.Consumer;

namespace Momolith.Modules.Messaging;

public sealed class MessagingModule : IModule
{
    private IHostApplicationBuilder Startup { get; }

    private MessagingModule(IHostApplicationBuilder startup)
    {
        startup.Services.AddHostedService<MessageConsumersRegisterer>();
        Startup = startup;
    }
    
    public static MessagingModule Create(IHostApplicationBuilder startup, IMessagingConsumerConfiguration configuration)
    {
        startup.Services.AddSingleton<IMessagingConsumerConfiguration>(configuration);
        return new MessagingModule(startup);
    }
    
    public static MessagingModule Create(IHostApplicationBuilder startup, Func<IServiceProvider, IMessagingConsumerConfiguration> factory)
    {
        startup.Services.AddSingleton<IMessagingConsumerConfiguration>(factory);
        return new MessagingModule(startup);
    }

    public static MessagingModule Create<TConfig>(IHostApplicationBuilder startup)
        where TConfig : class, IMessagingConsumerConfiguration
    {
        startup.Services.AddSingleton<IMessagingConsumerConfiguration, TConfig>();
        return new MessagingModule(startup);
    }
    
    public void AddConsumer<T>()
        where T : class, IMessageConsumerSetup
    {
        Startup.Services.AddSingleton<IMessageConsumerSetup, T>();
    }
}