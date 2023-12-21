using System.Text.Json;
using Explicit.Configuration;
using Explicit.Validation;
using Microsoft.Extensions.Configuration;
using Momolith.ConfigurationAsCode;
using OneOf;

namespace Momolith.Modules.Configuration.Default;

internal sealed class InMemoryConfigurationModifier : IConfigurationModifier
{
    private readonly IConfigurationManager _configuration;

    public InMemoryConfigurationModifier(IConfigurationManager configuration)
    {
        _configuration = configuration;
    }
    
    public void DownloadConfiguration(IConfigurationBuilder builder)
    {
    }

    public void UploadConfiguration<TOptions>(OneOf<Valid<TOptions>, SecretConfig<TOptions>> options)
        where TOptions : IConfigObject<TOptions>
    {
        options.Switch(valid =>
        {
            var section = TOptions.ConvertToSection(valid.ValidValue);
            var sectionValue = JsonSerializer.Serialize(section.Value);
            _configuration.AddInMemoryCollection(new Dictionary<string, string> { [TOptions.SectionName] = sectionValue }!);
        }, _ => { });
    }
}