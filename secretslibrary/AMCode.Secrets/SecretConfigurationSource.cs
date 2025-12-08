using Microsoft.Extensions.Configuration;

namespace AMCode.Secrets;

/// <summary>
/// Configuration source for loading secrets from ISecretService into IConfiguration
/// Allows existing code using IConfiguration to work with the secret management system
/// Secrets are loaded at startup and injected into IConfiguration, enabling backward compatibility
/// </summary>
public class SecretConfigurationSource : IConfigurationSource
{
    /// <summary>
    /// Builds the configuration provider for this source
    /// </summary>
    /// <param name="builder">The configuration builder</param>
    /// <returns>The configuration provider</returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new SecretConfigurationProvider(builder);
    }
}

/// <summary>
/// Extension methods for adding secret configuration to IConfigurationBuilder
/// </summary>
public static class SecretConfigurationExtensions
{
    /// <summary>
    /// Adds secret management configuration provider to the configuration builder
    /// This allows secrets from ISecretService to be injected into IConfiguration
    /// </summary>
    /// <param name="builder">The configuration builder</param>
    /// <returns>The configuration builder for chaining</returns>
    public static IConfigurationBuilder AddSecretManagementConfiguration(this IConfigurationBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Add(new SecretConfigurationSource());
        return builder;
    }
}
