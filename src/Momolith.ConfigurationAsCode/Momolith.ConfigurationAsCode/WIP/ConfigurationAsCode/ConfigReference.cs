using Microsoft.Extensions.Configuration;
using OneOf;
using OneOf.Types;
using Shared.Utils.Validation;

namespace Shared.Configuration;

public static partial class ConfigurationAsCode
{
    public sealed record Reference<TConfig>(
        Dictionary<string, OneOf<string, Secret, Manual>> Results,
        Func<IConfiguration, OneOf<Success, ValidationErrors>> Validation)
        where TConfig : IConfigElement
    {
        public Reference(
            string sectionName,
            OneOf<string, Secret, Manual> sectionValue,
            Func<IConfiguration, OneOf<Success, ValidationErrors>> validation) :
            this(new Dictionary<string, OneOf<string, Secret, Manual>>{[sectionName] = sectionValue}, validation)
        {
        }
    }

    public sealed record Secret(string SecretName);

    public sealed class Manual();
}