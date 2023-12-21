namespace Momolith.EnvironmentAsCode;


public sealed class EnvironmentName
{
    public string Name { get; }

    internal EnvironmentName(string name)
    {
        Name = name;
    }
}

public sealed class Environment<T>
    where T : IEnvironmentVersion
{
    
    public EnvironmentName EnvironmentName { get; }
    
    public string Name { get; }

    internal Environment()
    {
        Name = T.Name;
        EnvironmentName = new EnvironmentName(Name);
    }
}

public sealed class EnvironmentMatchResult<TResult>
{
    public EnvironmentName EnvironmentName { get; }

    public TResult Result { get; }
    
    internal EnvironmentMatchResult(TResult result, EnvironmentName environmentName)
    {
        Result = result;
        EnvironmentName = environmentName;
    }
}