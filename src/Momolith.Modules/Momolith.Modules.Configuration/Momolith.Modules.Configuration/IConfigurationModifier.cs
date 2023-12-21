using Microsoft.Extensions.Configuration;
using Momolith.ConfigurationAsCode;

namespace Momolith.Modules.Configuration;

public interface IConfigurationModifier : IConfigurationUploader
{
    public void DownloadConfiguration(IConfigurationBuilder builder);
}