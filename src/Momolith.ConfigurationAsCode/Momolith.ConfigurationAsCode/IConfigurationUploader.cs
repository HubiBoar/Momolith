using Explicit.Configuration;
using Explicit.Utils;
using Explicit.Validation;
using OneOf;

namespace Momolith.ConfigurationAsCode;

public interface IConfigurationUploader
{
    public void UploadConfiguration<TOptions>(OneOf<Valid<TOptions>, SecretConfig<TOptions>> options)
        where TOptions : IConfigObject<TOptions>;

    public void TryUpload<TOptions>(
        ConfigurationAsCodeEnabled configurationAsCode,
        IConfigurationAsCode<TOptions> configAsCode)
        where TOptions : IConfigObject<TOptions>
    {
        if (configurationAsCode.Enabled == false)
        {
            return;
        }
        
        var optionsResult = configAsCode.ConfigurationAsCode(new ConfigurationAsCodeContext<TOptions>());
        var options = optionsResult.Result;
        
        options.Switch(
            section => section.IsValid().Switch(
                validValue =>
                {
                    UploadConfiguration<TOptions>(validValue);
                },
                invalidValue => throw new Exception($"Invalid ConfigurationAsCode Option<{typeof(TOptions).GetTypeVerboseName()}>: '{options}', with message: {invalidValue.Message}")),
            manual => { },
            secretReference => UploadConfiguration<TOptions>(secretReference));
    }
}