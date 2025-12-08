using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace AMCode.Secrets;

/// <summary>
/// Configuration provider that loads secrets from ISecretService and injects them into IConfiguration
/// This allows existing code using IConfiguration to work with the secret management system
/// 
/// Note: By default, this provider has no secret mappings. Applications should extend this class
/// and override GetSecretMappings() to provide their own configuration-to-secret mappings.
/// </summary>
public class SecretConfigurationProvider : IConfigurationProvider
{
    private readonly IConfigurationBuilder _configurationBuilder;
    private readonly Dictionary<string, string> _secrets = new(StringComparer.OrdinalIgnoreCase);
    private bool _loaded = false;
    private readonly object _lock = new object();

    public SecretConfigurationProvider(IConfigurationBuilder configurationBuilder)
    {
        _configurationBuilder = configurationBuilder ?? throw new ArgumentNullException(nameof(configurationBuilder));
    }

    /// <summary>
    /// Attempts to get a configuration value
    /// </summary>
    public bool TryGet(string key, out string? value)
    {
        LoadSecrets();
        return _secrets.TryGetValue(key, out value);
    }

    /// <summary>
    /// Sets a configuration value (not used for this provider)
    /// </summary>
    public void Set(string key, string? value)
    {
        // This provider is read-only - secrets are loaded from ISecretService
    }

    /// <summary>
    /// Loads secrets from ISecretService
    /// This is called when TryGet is first invoked
    /// </summary>
    public void Load()
    {
        LoadSecrets();
    }

    /// <summary>
    /// Gets all child keys that have the specified prefix
    /// </summary>
    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath)
    {
        LoadSecrets();

        var prefix = parentPath == null ? string.Empty : parentPath + ConfigurationPath.KeyDelimiter;
        var prefixLength = prefix.Length;

        var keys = _secrets.Keys
            .Where(key => key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(key => key.Substring(prefixLength))
            .Where(key => key.IndexOf(ConfigurationPath.KeyDelimiter, StringComparison.Ordinal) < 0)
            .Concat(earlierKeys)
            .OrderBy(k => k, StringComparer.OrdinalIgnoreCase);

        return keys;
    }

    /// <summary>
    /// Gets a token that can be used to observe when this provider is reloaded
    /// This provider doesn't support reloading - secrets are loaded once at startup
    /// </summary>
    public IChangeToken GetReloadToken()
    {
        // Return a token that never signals change (secrets are loaded once at startup)
        return new Microsoft.Extensions.Primitives.CancellationChangeToken(System.Threading.CancellationToken.None);
    }

    /// <summary>
    /// Loads secrets from ISecretService into the configuration dictionary
    /// Only loads secrets for configuration keys that are empty or missing
    /// </summary>
    private void LoadSecrets()
    {
        if (_loaded)
        {
            return;
        }

        lock (_lock)
        {
            if (_loaded)
            {
                return;
            }

            try
            {
                // Build a temporary configuration to access existing config values
                // We need to build without this provider to avoid circular dependency
                var tempConfigBuilder = new ConfigurationBuilder();
                foreach (var source in _configurationBuilder.Sources)
                {
                    if (source is not SecretConfigurationSource)
                    {
                        tempConfigBuilder.Add(source);
                    }
                }
                var tempConfig = tempConfigBuilder.Build();

                // Build a temporary service provider to get ISecretService
                var services = new ServiceCollection();
                services.AddSingleton<IConfiguration>(tempConfig);
                services.AddLogging();
                
                // Add secret management services
                AMCode.Secrets.Extensions.SecretManagementExtensions.AddSecretManagement(services, tempConfig);
                
                using var serviceProvider = services.BuildServiceProvider();
                var secretService = serviceProvider.GetRequiredService<ISecretService>();
                var logger = serviceProvider.GetRequiredService<ILogger<SecretConfigurationProvider>>();

                logger.LogDebug("Loading secrets from secret management provider into IConfiguration");

                // Define secret mappings: configuration path -> secret name (kebab-case)
                // Note: These mappings are application-specific and should be made configurable for a generic library
                var secretMappings = GetSecretMappings();

                // Load all secrets
                foreach (var mapping in secretMappings)
                {
                    var configPath = mapping.Key;
                    var secretName = mapping.Value;

                    // Check if the configuration value is already set (non-empty)
                    var existingValue = tempConfig[configPath];
                    if (!string.IsNullOrWhiteSpace(existingValue))
                    {
                        logger.LogDebug("Configuration path '{ConfigPath}' already has a value, skipping secret load", configPath);
                        continue;
                    }

                    // Load secret from ISecretService (synchronously at startup)
                    var secretValue = secretService.GetSecretAsync(secretName).GetAwaiter().GetResult();

                    if (!string.IsNullOrWhiteSpace(secretValue))
                    {
                        _secrets[configPath] = secretValue;
                        logger.LogDebug("Loaded secret '{SecretName}' into configuration path '{ConfigPath}'", secretName, configPath);
                    }
                    else
                    {
                        logger.LogTrace("Secret '{SecretName}' not found or empty, skipping configuration path '{ConfigPath}'", secretName, configPath);
                    }
                }

                logger.LogInformation("Loaded {Count} secrets from secret management provider into IConfiguration", _secrets.Count);
                _loaded = true;
            }
            catch (Exception ex)
            {
                // Log error but don't fail - configuration provider errors shouldn't crash the app
                var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<SecretConfigurationProvider>();
                logger.LogError(ex, "Error loading secrets from secret management provider. Existing IConfiguration will be used.");
                _loaded = true; // Mark as loaded to prevent retry loops
            }
        }
    }

    /// <summary>
    /// Maps configuration paths to secret names
    /// Configuration path format: "Section:Subsection:Key"
    /// Secret name format (kebab-case): "section-subsection-key"
    /// 
    /// Note: This returns an empty dictionary by default. Applications should extend this class
    /// and override GetSecretMappings() to provide their own configuration-to-secret mappings.
    /// </summary>
    protected virtual Dictionary<string, string> GetSecretMappings()
    {
        // Return empty mappings for generic library
        // Applications should extend this class and override this method to provide their mappings
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }
}
