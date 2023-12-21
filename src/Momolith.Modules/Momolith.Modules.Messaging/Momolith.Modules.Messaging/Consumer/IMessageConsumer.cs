using OneOf;
using OneOf.Types;

namespace Momolith.Modules.Messaging.Consumer;

public interface IMessageConsumerSetup
{
    protected IMessagingConsumerConfiguration Config { get; }

    internal Task Setup();
}

public interface IMessageConsumer<in T> : IMessageConsumerSetup
    where T : class, IMessage
{
    public delegate Task<OneOf<Success, ConsumerError>> MessageMethod(T message, IConsumerLogger logger);
    
    Task IMessageConsumerSetup.Setup()
    {
        return Config.RegisterConsumer(OnMessage, this);
    }

    public sealed Task Send(T message)
    {
        return Config.SendMessage(message, OnMessage, this);
    }

    protected Task<OneOf<Success, ConsumerError>> OnMessage(T message, IConsumerLogger logger);
}