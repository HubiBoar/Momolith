namespace Momolith.Modules.Messaging.Consumer;

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