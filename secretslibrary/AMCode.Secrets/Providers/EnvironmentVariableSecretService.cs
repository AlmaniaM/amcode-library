using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AMCode.Secrets.Providers;

/// <summary>
/// Local secret service implementation for development
/// Reads secrets from ASP.NET Core IConfiguration, which includes (in priority order):
/// 1. User Secrets (local development - recommended for sensitive values)
/// 2. Environment variables (system-level)
/// 3. appsettings.json files (non-sensitive configuration)
/// 
/// For local development, use User Secrets: dotnet user-secrets set "AI:OpenAI:ApiKey" "your-key"
/// </summary>
public class EnvironmentVariableSecretService : ISecretService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EnvironmentVariableSecretService> _logger;

    public EnvironmentVariableSecretService(
        IConfiguration configuration,
        ILogger<EnvironmentVariableSecretService> logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves a secret value from User Secrets, environment variables, or configuration
    /// Priority: User Secrets > Environment Variables > appsettings.json
    /// </summary>
    /// <param name="secretName">Secret name in kebab-case (e.g., "ai-openai-apikey") or double-underscore format (e.g., "AI__OpenAI__ApiKey")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The secret value if found, null if secret does not exist</returns>
    public Task<string?> GetSecretAsync(string secretName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(secretName))
        {
            _logger.LogWarning("Attempted to retrieve secret with empty or null name");
            return Task.FromResult<string?>(null);
        }

        try
        {
            // Map secret name to configuration path
            // Cloud format: "ai-openai-apikey" -> "ai__openai__apikey" -> "AI:OpenAI:ApiKey"
            // Direct format: "AI__OpenAI__ApiKey" -> "AI:OpenAI:ApiKey"
            var configPath = MapSecretNameToConfigPath(secretName);

            var secretValue = _configuration.GetValue<string>(configPath);

            if (string.IsNullOrEmpty(secretValue))
            {
                _logger.LogWarning("Secret '{SecretName}' (mapped to config path '{ConfigPath}') not found in User Secrets, environment variables, or configuration",
                    secretName, configPath);
                return Task.FromResult<string?>(null);
            }

            _logger.LogDebug("Successfully retrieved secret '{SecretName}' from User Secrets or configuration", secretName);
            return Task.FromResult<string?>(secretValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving secret '{SecretName}' from User Secrets or configuration", secretName);
            return Task.FromResult<string?>(null);
        }
    }

    /// <summary>
    /// Retrieves multiple secrets in batch
    /// </summary>
    /// <param name="secretNames">Collection of secret names to retrieve</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dictionary of secret names to their values. Missing secrets are not included</returns>
    public async Task<Dictionary<string, string>> GetSecretsAsync(IEnumerable<string> secretNames, CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<string, string>();

        if (secretNames == null)
        {
            _logger.LogWarning("Attempted to retrieve secrets with null collection");
            return result;
        }

        foreach (var secretName in secretNames)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            var secretValue = await GetSecretAsync(secretName, cancellationToken);
            if (!string.IsNullOrEmpty(secretValue))
            {
                result[secretName] = secretValue;
            }
        }

        _logger.LogDebug("Retrieved {Count} of {Total} requested secrets from User Secrets or configuration",
            result.Count, secretNames.Count());

        return result;
    }

    /// <summary>
    /// Checks if a secret exists
    /// </summary>
    /// <param name="secretName">The name of the secret to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the secret exists, false otherwise</returns>
    public async Task<bool> SecretExistsAsync(string secretName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(secretName))
        {
            return false;
        }

        var secretValue = await GetSecretAsync(secretName, cancellationToken);
        return !string.IsNullOrEmpty(secretValue);
    }

    /// <summary>
    /// Maps secret name from cloud format to ASP.NET Core configuration path
    /// </summary>
    /// <param name="secretName">Secret name in kebab-case (e.g., "ai-openai-apikey") or double-underscore format (e.g., "AI__OpenAI__ApiKey")</param>
    /// <returns>Configuration path (e.g., "AI:OpenAI:ApiKey")</returns>
    /// <remarks>
    /// Mapping rules:
    /// 1. Replace hyphens with double underscores: "ai-openai-apikey" -> "ai__openai__apikey"
    /// 2. Single underscores are preserved (if already double-underscore format, keep it)
    /// 3. ASP.NET Core IConfiguration automatically converts double underscores to colons
    ///    So "AI__OpenAI__ApiKey" becomes "AI:OpenAI:ApiKey" in configuration
    /// 
    /// User Secrets format: Use colon notation directly (e.g., "AI:OpenAI:ApiKey")
    /// Environment variable format: Use double underscores (e.g., "AI__OpenAI__ApiKey")
    /// </remarks>
    private string MapSecretNameToConfigPath(string secretName)
    {
        // If already in double-underscore format (e.g., "AI__OpenAI__ApiKey"), use it directly
        if (secretName.Contains("__"))
        {
            return secretName;
        }

        // Convert kebab-case to double-underscore format
        // "ai-openai-apikey" -> "ai__openai__apikey"
        // ASP.NET Core IConfiguration will convert double underscores to colons: "AI:OpenAI:ApiKey"
        return secretName.Replace("-", "__");
    }
}
