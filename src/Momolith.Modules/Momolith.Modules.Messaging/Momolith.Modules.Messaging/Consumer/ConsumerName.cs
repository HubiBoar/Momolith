using Explicit.Utils;

namespace Momolith.Modules.Messaging.Consumer;

public record ConsumerName
{
    public string Value => ConsumerType.GetTypeVerboseName();

    private Type ConsumerType { get; }

    private ConsumerName(Type consumerType)
    {
        ConsumerType = consumerType;
    }

    public static ConsumerName GetConsumerName(IMessageConsumerSetup consumer)
    {
        return new ConsumerName(consumer.GetType());
    }
}