using Explicit.Configuration;
using Explicit.Validation.FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Momolith.Startup.Environment;

public sealed record EnvironmentAsCodeEnv(string EnvironmentName);

public sealed record EnvironmentParameters(EnvironmentName Name, int Index);

public interface IEnvironments
{
    public static abstract IReadOnlyCollection<EnvironmentName> Environments { get; }
}

public abstract class EnvironmentAsCodeBase<TEnvs>
    where TEnvs : IEnvironments
{
    public EnvironmentName Name => Parameters.Name;

    protected EnvironmentParameters Parameters { get; }

    protected EnvironmentAsCodeBase(EnvironmentAsCodeEnv environment)
    {
        var index = TEnvs.Environments.ToList().FindIndex(x => x.Name == environment.EnvironmentName);

        if (index == -1)
        {
            throw new Exception("Momolith.Environment: Does not match any of environments");
        }

        var environmentName = TEnvs.Environments.ElementAt(index);

        Parameters = new EnvironmentParameters(environmentName, index);
    }
    
    protected static EnvironmentAsCodeEnv DefaultEnvironmentSetting(IConfiguration configuration)
    {
        return configuration.GetValid<EnvironmentSetting>().Basic.Match(
            valid => new EnvironmentAsCodeEnv(valid.Value),
            errors => throw errors.ToException());
    }
    
    private sealed class EnvironmentSetting : IConfigValue<EnvironmentSetting, string, IsNotEmpty>
    {
        public static string SectionName { get; } = "Momolith.EnvironmentAsCode";

        public string Value { get; init; }
    }
}