namespace Momolith.Modules.Messaging.Consumer;

public sealed class ConsumerError
{
    public string Message { get; }

    public ConsumerError(string message)
    {
        Message = message;
    }

    public ConsumerError(Exception exception)
    {
        Message = exception.ToString();
    }
    
    public ConsumerError(string prefix, Exception exception)
    {
        Message = $"{prefix} :: {exception}";
    }
}