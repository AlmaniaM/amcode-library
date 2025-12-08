using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMCode.Secrets.Providers;

/// <summary>
/// GCP Secret Manager secret service implementation
/// Uses Application Default Credentials (Cloud Run) or service account JSON for authentication
/// </summary>
public class GCPSecretManagerService : ISecretService
{
    private readonly SecretManagerServiceClient? _secretClient;
    private readonly ILogger<GCPSecretManagerService> _logger;
    private readonly GCPSecretManagerConfig _config;
    private readonly string? _projectId;

    public GCPSecretManagerService(
        IOptions<SecretManagementConfig> config,
        ILogger<GCPSecretManagerService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (config == null || config.Value == null)
        {
            _logger.LogWarning("SecretManagementConfig is null. GCP Secret Manager provider will not be available.");
            _config = new GCPSecretManagerConfig();
            _secretClient = null;
            _projectId = null;
            return;
        }

        _config = config.Value.GCPSecretManager;

        if (string.IsNullOrWhiteSpace(_config.ProjectId))
        {
            _logger.LogWarning(
                "GCP Project ID is not configured. " +
                "GCP Secret Manager provider will not be available. " +
                "To enable, configure 'SecretManagement:GCPSecretManager:ProjectId' in appsettings.json");
            _secretClient = null;
            _projectId = null;
            return;
        }

        _projectId = _config.ProjectId;

        try
        {
            // Configure credentials
            Google.Cloud.SecretManager.V1.SecretManagerServiceClient client;

            if (!string.IsNullOrWhiteSpace(_config.CredentialsPath))
            {
                // Use explicit service account JSON file if provided
                _logger.LogInformation("Using service account JSON credentials from '{CredentialsPath}' for GCP Secret Manager",
                    _config.CredentialsPath);
                // Note: SecretManagerServiceClient.Create() uses Application Default Credentials by default
                // For explicit credentials path, you would use:
                // var credentials = GoogleCredential.FromFile(_config.CredentialsPath);
                // client = SecretManagerServiceClient.Create(SecretManagerServiceClient.Create(), new SecretManagerServiceClient.ClientCreationSettings(credentials: credentials));
                // For now, we'll use the default which respects GOOGLE_APPLICATION_CREDENTIALS environment variable
                client = SecretManagerServiceClient.Create();
            }
            else
            {
                // Use Application Default Credentials (ADC)
                // ADC tries the following in order:
                // 1. GOOGLE_APPLICATION_CREDENTIALS environment variable (path to service account JSON)
                // 2. Google Cloud SDK credentials (gcloud auth application-default login)
                // 3. Google Cloud Compute Engine default service account (when running on GCE/GKE)
                // 4. Google Cloud Run default service account (when running on Cloud Run)
                _logger.LogInformation("Using Application Default Credentials for GCP Secret Manager");
                client = SecretManagerServiceClient.Create();
            }

            _secretClient = client;
            _logger.LogInformation("GCP Secret Manager client initialized for project: {ProjectId}", _projectId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize GCP Secret Manager client. Provider will not be available.");
            _secretClient = null;
        }
    }

    /// <summary>
    /// Retrieves a secret value from GCP Secret Manager
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

        if (_secretClient == null || string.IsNullOrWhiteSpace(_projectId))
        {
            _logger.LogWarning("GCP Secret Manager client is not configured. Cannot retrieve secret '{SecretName}'.", secretName);
            return null;
        }

        try
        {
            _logger.LogDebug("Retrieving secret '{SecretName}' from GCP Secret Manager (project: {ProjectId})",
                secretName, _projectId);

            // Build the secret version name
            // Format: projects/{project}/secrets/{secret}/versions/latest
            var secretVersionName = new SecretVersionName(_projectId, secretName, "latest");

            // Access the secret version
            var response = await _secretClient.AccessSecretVersionAsync(secretVersionName, cancellationToken);

            if (response?.Payload?.Data == null)
            {
                _logger.LogWarning("Secret '{SecretName}' not found or has no data in GCP Secret Manager", secretName);
                return null;
            }

            // Get the secret value as a string
            var secretValue = response.Payload.Data.ToStringUtf8();

            _logger.LogDebug("Successfully retrieved secret '{SecretName}' from GCP Secret Manager", secretName);
            return secretValue;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            // Secret not found - return null (graceful degradation)
            _logger.LogWarning("Secret '{SecretName}' not found in GCP Secret Manager (404): {Message}",
                secretName, ex.Message);
            return null;
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.PermissionDenied)
        {
            // Permission denied
            _logger.LogError("Permission denied accessing secret '{SecretName}' in GCP Secret Manager: {Message}",
                secretName, ex.Message);
            return null;
        }
        catch (RpcException ex)
        {
            // Other RPC errors
            _logger.LogError(ex, "RPC error retrieving secret '{SecretName}' from GCP Secret Manager (Status: {StatusCode})",
                secretName, ex.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving secret '{SecretName}' from GCP Secret Manager",
                secretName);
            return null;
        }
    }

    /// <summary>
    /// Retrieves multiple secrets in batch from GCP Secret Manager
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

        // GCP Secret Manager doesn't have a batch API, so we retrieve secrets sequentially
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

        _logger.LogDebug("Retrieved {Count} of {Total} requested secrets from GCP Secret Manager",
            result.Count, secretNames.Count());

        return result;
    }

    /// <summary>
    /// Checks if a secret exists in GCP Secret Manager
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
            _logger.LogWarning(ex, "Error checking existence of secret '{SecretName}' in GCP Secret Manager", secretName);
            return false;
        }
    }
}
