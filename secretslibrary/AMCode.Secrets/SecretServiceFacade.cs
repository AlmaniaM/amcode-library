using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMCode.Secrets;

/// <summary>
/// Facade implementation for secret management
/// Selects and delegates to the configured secret provider
/// </summary>
public class SecretServiceFacade : ISecretService
{
    private readonly ISecretService _selectedProvider;
    private readonly SecretProvider _provider;
    private readonly ILogger<SecretServiceFacade> _logger;

    public SecretServiceFacade(
        IOptions<SecretManagementConfig> config,
        IEnumerable<ISecretService> providers,
        ILogger<SecretServiceFacade> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (config == null || config.Value == null)
        {
            throw new ArgumentNullException(nameof(config), "SecretManagementConfig is required");
        }

        var configuration = config.Value;
        _provider = configuration.Provider;

        // Select the appropriate provider based on configuration
        _selectedProvider = SelectProvider(_provider, providers);

        _logger.LogInformation("Secret management initialized with provider: {Provider}", _provider);
    }

    /// <summary>
    /// Retrieves a secret value by name from the selected provider
    /// </summary>
    public async Task<string?> GetSecretAsync(string secretName, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Retrieving secret '{SecretName}' from provider {Provider}", secretName, _provider);
        return await _selectedProvider.GetSecretAsync(secretName, cancellationToken);
    }

    /// <summary>
    /// Retrieves multiple secrets by name from the selected provider
    /// </summary>
    public async Task<Dictionary<string, string>> GetSecretsAsync(IEnumerable<string> secretNames, CancellationToken cancellationToken = default)
    {
        if (secretNames == null)
        {
            _logger.LogWarning("Attempted to retrieve secrets with null collection");
            return new Dictionary<string, string>();
        }

        _logger.LogDebug("Retrieving {Count} secrets from provider {Provider}", secretNames.Count(), _provider);
        return await _selectedProvider.GetSecretsAsync(secretNames, cancellationToken);
    }

    /// <summary>
    /// Checks if a secret exists in the selected provider
    /// </summary>
    public async Task<bool> SecretExistsAsync(string secretName, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Checking existence of secret '{SecretName}' in provider {Provider}", secretName, _provider);
        return await _selectedProvider.SecretExistsAsync(secretName, cancellationToken);
    }

    /// <summary>
    /// Selects the appropriate provider based on configuration
    /// </summary>
    /// <param name="provider">The configured provider type</param>
    /// <param name="providers">All registered provider implementations</param>
    /// <returns>The selected provider implementation</returns>
    /// <exception cref="InvalidOperationException">Thrown if provider is not configured or not found</exception>
    private ISecretService SelectProvider(SecretProvider provider, IEnumerable<ISecretService> providers)
    {
        if (providers == null)
        {
            throw new InvalidOperationException("No secret providers registered. Ensure AddSecretManagement is called during service registration.");
        }

        // Filter providers by type matching the configured provider
        var providerList = providers.ToList();
        ISecretService? selectedProvider = null;

        switch (provider)
        {
            case SecretProvider.Environment:
                selectedProvider = providerList.OfType<Providers.EnvironmentVariableSecretService>().FirstOrDefault();
                break;

            case SecretProvider.AzureKeyVault:
                selectedProvider = providerList.OfType<Providers.AzureKeyVaultSecretService>().FirstOrDefault();
                break;

            case SecretProvider.AWSSecretsManager:
                selectedProvider = providerList.OfType<Providers.AWSSecretsManagerService>().FirstOrDefault();
                break;

            case SecretProvider.GCPSecretManager:
                selectedProvider = providerList.OfType<Providers.GCPSecretManagerService>().FirstOrDefault();
                break;

            default:
                throw new InvalidOperationException(
                    $"Invalid secret provider '{provider}'. " +
                    $"Valid providers are: Environment, AzureKeyVault, AWSSecretsManager, GCPSecretManager");
        }

        if (selectedProvider == null)
        {
            throw new InvalidOperationException(
                $"Secret provider '{provider}' is configured but not registered. " +
                $"Ensure the provider implementation is registered via AddSecretManagement.");
        }

        return selectedProvider;
    }
}
