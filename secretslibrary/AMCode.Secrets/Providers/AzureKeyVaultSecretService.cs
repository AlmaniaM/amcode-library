using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMCode.Secrets.Providers;

/// <summary>
/// Azure Key Vault secret service implementation
/// Uses DefaultAzureCredential for authentication (supports Managed Identity, Azure CLI, Visual Studio, etc.)
/// </summary>
public class AzureKeyVaultSecretService : ISecretService
{
    private readonly SecretClient? _secretClient;
    private readonly ILogger<AzureKeyVaultSecretService> _logger;
    private readonly AzureKeyVaultConfig _config;

    public AzureKeyVaultSecretService(
        IOptions<SecretManagementConfig> config,
        ILogger<AzureKeyVaultSecretService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (config == null || config.Value == null)
        {
            _logger.LogWarning("SecretManagementConfig is null. Azure Key Vault provider will not be available.");
            _config = new AzureKeyVaultConfig();
            _secretClient = null;
            return;
        }

        _config = config.Value.AzureKeyVault;

        if (string.IsNullOrWhiteSpace(_config.VaultUri))
        {
            _logger.LogWarning(
                "Azure Key Vault URI is not configured. " +
                "Azure Key Vault provider will not be available. " +
                "To enable, configure 'SecretManagement:AzureKeyVault:VaultUri' in appsettings.json");
            _secretClient = null;
            return;
        }

        try
        {
            // Create SecretClient with DefaultAzureCredential
            // DefaultAzureCredential tries the following authentication methods in order:
            // 1. Environment variables (AZURE_CLIENT_ID, AZURE_CLIENT_SECRET, AZURE_TENANT_ID)
            // 2. Managed Identity (when running in Azure)
            // 3. Azure CLI (when logged in locally)
            // 4. Visual Studio credentials
            // 5. Azure PowerShell credentials
            TokenCredential credential;

            if (!string.IsNullOrWhiteSpace(_config.TenantId) &&
                !string.IsNullOrWhiteSpace(_config.ClientId) &&
                !string.IsNullOrWhiteSpace(_config.ClientSecret))
            {
                // Use explicit service principal credentials if provided
                _logger.LogInformation("Using explicit service principal credentials for Azure Key Vault");
                credential = new ClientSecretCredential(
                    _config.TenantId,
                    _config.ClientId,
                    _config.ClientSecret);
            }
            else
            {
                // Use DefaultAzureCredential for automatic credential resolution
                _logger.LogInformation("Using DefaultAzureCredential for Azure Key Vault authentication");
                credential = new DefaultAzureCredential();
            }

            _secretClient = new SecretClient(new Uri(_config.VaultUri), credential);
            _logger.LogInformation("Azure Key Vault client initialized for vault: {VaultUri}", _config.VaultUri);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize Azure Key Vault client. Provider will not be available.");
            _secretClient = null;
        }
    }

    /// <summary>
    /// Retrieves a secret value from Azure Key Vault
    /// </summary>
    /// <param name="secretName">Secret name in kebab-case (e.g., "ai-openai-apikey")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The secret value if found, null if secret does not exist</returns>
    public async Task<string?> GetSecretAsync(string secretName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(secretName))
        {
            _logger.LogWarning("Attempted to retrieve secret with empty or null name");
            return null;
        }

        if (_secretClient == null)
        {
            _logger.LogWarning("Azure Key Vault client is not configured. Cannot retrieve secret '{SecretName}'.", secretName);
            return null;
        }

        try
        {
            _logger.LogDebug("Retrieving secret '{SecretName}' from Azure Key Vault", secretName);

            // Azure Key Vault secret names are case-insensitive and use hyphens
            var secretResponse = await _secretClient.GetSecretAsync(secretName, cancellationToken: cancellationToken);

            if (secretResponse?.Value?.Value == null)
            {
                _logger.LogWarning("Secret '{SecretName}' not found in Azure Key Vault", secretName);
                return null;
            }

            _logger.LogDebug("Successfully retrieved secret '{SecretName}' from Azure Key Vault", secretName);
            return secretResponse.Value.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            // Secret not found - return null (graceful degradation)
            _logger.LogWarning("Secret '{SecretName}' not found in Azure Key Vault (404)", secretName);
            return null;
        }
        catch (RequestFailedException ex)
        {
            // Other request failures (authentication, network, etc.)
            _logger.LogError(ex, "Error retrieving secret '{SecretName}' from Azure Key Vault (Status: {Status})",
                secretName, ex.Status);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving secret '{SecretName}' from Azure Key Vault", secretName);
            return null;
        }
    }

    /// <summary>
    /// Retrieves multiple secrets in batch from Azure Key Vault
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

        // Azure Key Vault doesn't have a batch API, so we retrieve secrets sequentially
        // This could be optimized with parallel requests if needed
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

        _logger.LogDebug("Retrieved {Count} of {Total} requested secrets from Azure Key Vault",
            result.Count, secretNames.Count());

        return result;
    }

    /// <summary>
    /// Checks if a secret exists in Azure Key Vault
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

        try
        {
            // Try to get the secret - if it doesn't exist, GetSecretAsync returns null
            var secretValue = await GetSecretAsync(secretName, cancellationToken);
            return !string.IsNullOrEmpty(secretValue);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error checking existence of secret '{SecretName}' in Azure Key Vault", secretName);
            return false;
        }
    }
}
