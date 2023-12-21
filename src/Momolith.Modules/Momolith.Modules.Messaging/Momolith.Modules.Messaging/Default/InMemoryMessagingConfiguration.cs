using Microsoft.Extensions.Logging;
using Momolith.Modules.Messaging.Consumer;

namespace Momolith.Modules.Messaging.Default;

internal sealed class InMemoryMessagingConfiguration : IMessagingConsumerConfiguration
{
    private readonly ILoggerFactory _loggerFactory;

    public InMemoryMessagingConfiguration(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public Task SendMessage<T>(
        T message,
        IMessageConsumer<T>.MessageMethod onMessage,
        IMessageConsumer<T> consumer)
        where T : class, IMessage
    {
        var consumerName = ConsumerName.GetConsumerName(consumer);
        var messageId = $"InMemory-{Guid.NewGuid().ToString()}";
        
        var consumerLogger = IConsumerLogger.CreateConsumerLogger(
            _loggerFactory,
            messageId,
            consumerName);
        
        return onMessage.Invoke(message, consumerLogger);
    }

    public Task RegisterConsumer<T>(
        IMessageConsumer<T>.MessageMethod onMessage,
        IMessageConsumer<T> consumer)
        where T : class, IMessage
    {
        return Task.CompletedTask;
    }
}