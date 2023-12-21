using Microsoft.Extensions.Logging;

namespace Momolith.Modules.Messaging.Consumer;

public interface IConsumerLogger : ILogger
{
    public static IConsumerLogger CreateConsumerLogger(
        ILoggerFactory factory,
        string messageId,
        ConsumerName consumerName)
    {
        return new ConsumerLogger(factory, messageId, consumerName);
    }
}

internal class ConsumerLogger : IConsumerLogger
{
    private readonly ILogger _logger;
    private readonly string _messageId;
    private readonly ConsumerName _consumerName;

    public ConsumerLogger(ILoggerFactory loggerFactory, string messageId, ConsumerName consumerName)
    {
        _logger = loggerFactory.CreateLogger(consumerName.Value);
        _messageId = messageId;
        _consumerName = consumerName;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return _logger.BeginScope(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, MessageFormatter);
    }

    private string MessageFormatter<TState>(TState state, Exception? error)
    {
        return $"{_messageId} :: {_consumerName.Value} :: {state?.ToString()}";
    }
}