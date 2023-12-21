namespace Momolith.Startup.EnvironmentAsCode;

public interface IEnvironmentVersion
{
    public static abstract string Name { get; }
}