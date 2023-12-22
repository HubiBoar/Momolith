using Microsoft.Extensions.Hosting;

namespace Momolith.Modules.Messaging.Consumer;

internal sealed class MessageConsumersRegisterer : BackgroundService
{
    private readonly IReadOnlyCollection<IMessageConsumerSetup> _messageConsumerSetups;

    public MessageConsumersRegisterer(IEnumerable<IMessageConsumerSetup> messageConsumerSetups)
    {
        _messageConsumerSetups = messageConsumerSetups.ToArray();
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var consumer in _messageConsumerSetups)
        {
            await consumer.Setup();
        }
    }
}