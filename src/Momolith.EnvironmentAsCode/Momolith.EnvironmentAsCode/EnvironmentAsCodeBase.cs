using Explicit.Configuration;
using Explicit.Validation.FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Momolith.EnvironmentAsCode;

public sealed record EnvironmentAsCodeName(string EnvironmentName);

public sealed record EnvironmentParameters(EnvironmentName Name, int Index);

public interface IEnvironmentAsCode
{
    public static abstract IReadOnlyCollection<EnvironmentName> Environments { get; }
}

public abstract class EnvironmentAsCodeBase<TEnvs>
    where TEnvs : IEnvironmentAsCode
{
    public EnvironmentName Name => Parameters.Name;

    protected EnvironmentParameters Parameters { get; }

    protected EnvironmentAsCodeBase(EnvironmentAsCodeName environment)
    {
        var index = TEnvs.Environments.ToList().FindIndex(x => x.Name == environment.EnvironmentName);

        if (index == -1)
        {
            throw new Exception("Momolith.Environment: Does not match any of environments");
        }

        var environmentName = TEnvs.Environments.ElementAt(index);

        Parameters = new EnvironmentParameters(environmentName, index);
    }
    
    protected static EnvironmentAsCodeName DefaultEnvironmentSetting(IConfiguration configuration)
    {
        return configuration.GetValid<EnvironmentSetting>().Basic.Match(
            valid => new EnvironmentAsCodeName(valid.Value),
            errors => throw errors.ToException());
    }
    
    private sealed class EnvironmentSetting : IConfigValue<EnvironmentSetting, string, IsNotEmpty>
    {
        public static string SectionName { get; } = "Momolith.EnvironmentAsCode";

        public string Value { get; init; }
    }
}