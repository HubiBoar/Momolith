namespace Momolith.Startup.Environment;

public abstract class EnvironmentAsCodeBase<T0, T1> : EnvironmentAsCodeBase<EnvironmentAsCodeBase<T0, T1>>, IEnvironments
    where T0 : IEnvironmentVersion
    where T1 : IEnvironmentVersion
{
    public static IReadOnlyCollection<EnvironmentName> Environments { get; } = new[]
    {
        new EnvironmentName(T0.Name),
        new EnvironmentName(T1.Name)
    };
    
    protected EnvironmentAsCodeBase(EnvironmentAsCodeEnv environment) : base(environment)
    {
    }

    public EnvironmentMatchResult<TReturnValue> Match<TReturnValue>(
        Func<Environment<T0>, TReturnValue> onT0,
        Func<Environment<T1>, TReturnValue> onT1)
    {
        return Parameters.Index switch
        {
            0 => new EnvironmentMatchResult<TReturnValue>(onT0(new Environment<T0>()), Name),
            _ => new EnvironmentMatchResult<TReturnValue>(onT1(new Environment<T1>()), Name)
        };
    }
}

public abstract class EnvironmentAsCodeBase<T0, T1, T2> : EnvironmentAsCodeBase<EnvironmentAsCodeBase<T0, T1, T2>>, IEnvironments
    where T0 : IEnvironmentVersion
    where T1 : IEnvironmentVersion
    where T2 : IEnvironmentVersion
{
    public static IReadOnlyCollection<EnvironmentName> Environments { get; } = new[]
    {
        new EnvironmentName(T0.Name),
        new EnvironmentName(T1.Name),
        new EnvironmentName(T2.Name)
    };
    
    protected EnvironmentAsCodeBase(EnvironmentAsCodeEnv environment) : base(environment)
    {
    }

    public EnvironmentMatchResult<TReturnValue> Match<TReturnValue>(
        Func<Environment<T0>, TReturnValue> onT0,
        Func<Environment<T1>, TReturnValue> onT1,
        Func<Environment<T2>, TReturnValue> onT2)
    {
        return Parameters.Index switch
        {
            0 => new EnvironmentMatchResult<TReturnValue>(onT0(new Environment<T0>()), Name),
            1 => new EnvironmentMatchResult<TReturnValue>(onT1(new Environment<T1>()), Name),
            _ => new EnvironmentMatchResult<TReturnValue>(onT2(new Environment<T2>()), Name),
        };
    }
}


public abstract class EnvironmentAsCodeBase<T0, T1, T2, T3> : EnvironmentAsCodeBase<EnvironmentAsCodeBase<T0, T1, T2, T3>>, IEnvironments
    where T0 : IEnvironmentVersion
    where T1 : IEnvironmentVersion
    where T2 : IEnvironmentVersion
    where T3 : IEnvironmentVersion
{
    public static IReadOnlyCollection<EnvironmentName> Environments { get; } = new[]
    {
        new EnvironmentName(T0.Name),
        new EnvironmentName(T1.Name),
        new EnvironmentName(T2.Name),
        new EnvironmentName(T3.Name)
    };
    
    protected EnvironmentAsCodeBase(EnvironmentAsCodeEnv environment) : base(environment)
    {
    }

    public EnvironmentMatchResult<TReturnValue> Match<TReturnValue>(
        Func<Environment<T0>, TReturnValue> onT0,
        Func<Environment<T1>, TReturnValue> onT1,
        Func<Environment<T2>, TReturnValue> onT2,
        Func<Environment<T3>, TReturnValue> onT3)
    {
        return Parameters.Index switch
        {
            0 => new EnvironmentMatchResult<TReturnValue>(onT0(new Environment<T0>()), Name),
            1 => new EnvironmentMatchResult<TReturnValue>(onT1(new Environment<T1>()), Name),
            2 => new EnvironmentMatchResult<TReturnValue>(onT2(new Environment<T2>()), Name),
            _ => new EnvironmentMatchResult<TReturnValue>(onT3(new Environment<T3>()), Name),
        };
    }
}