using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Momolith.Modules.Messaging.Consumer;

namespace Momolith.Modules.Messaging;

public sealed class MessagingComponent : IModule
{
    private IHostApplicationBuilder Startup { get; }

    private MessagingComponent(IHostApplicationBuilder startup)
    {
        startup.Services.AddHostedService<MessageConsumersRegisterer>();
        Startup = startup;
    }
    
    public static MessagingComponent Create(IHostApplicationBuilder startup, IMessagingConsumerConfiguration configuration)
    {
        startup.Services.AddSingleton<IMessagingConsumerConfiguration>(configuration);
        return new MessagingComponent(startup);
    }
    
    public static MessagingComponent Create(IHostApplicationBuilder startup, Func<IServiceProvider, IMessagingConsumerConfiguration> factory)
    {
        startup.Services.AddSingleton<IMessagingConsumerConfiguration>(factory);
        return new MessagingComponent(startup);
    }

    public static MessagingComponent Create<TConfig>(IHostApplicationBuilder startup)
        where TConfig : class, IMessagingConsumerConfiguration
    {
        startup.Services.AddSingleton<IMessagingConsumerConfiguration, TConfig>();
        return new MessagingComponent(startup);
    }
    
    public void AddConsumer<T>()
        where T : class, IMessageConsumerSetup
    {
        Startup.Services.AddSingleton<IMessageConsumerSetup, T>();
    }
}


public interface IMessagingConsumerConfiguration
{
    Task SendMessage<T>(
        T message,
        IMessageConsumer<T>.MessageMethod onMessage,
        IMessageConsumer<T> consumer)
        where T : class, IMessage;
    
    Task RegisterConsumer<T>(
        IMessageConsumer<T>.MessageMethod onMessage,
        IMessageConsumer<T> consumer)
        where T : class, IMessage;
}