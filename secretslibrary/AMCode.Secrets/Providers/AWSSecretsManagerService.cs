using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMCode.Secrets.Providers;

/// <summary>
/// AWS Secrets Manager secret service implementation
/// Uses IAM roles (ECS/Lambda) or access keys for authentication
/// </summary>
public class AWSSecretsManagerService : ISecretService
{
    private readonly IAmazonSecretsManager? _secretsManagerClient;
    private readonly ILogger<AWSSecretsManagerService> _logger;
    private readonly AWSSecretsManagerConfig _config;

    public AWSSecretsManagerService(
        IOptions<SecretManagementConfig> config,
        ILogger<AWSSecretsManagerService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (config == null || config.Value == null)
        {
            _logger.LogWarning("SecretManagementConfig is null. AWS Secrets Manager provider will not be available.");
            _config = new AWSSecretsManagerConfig();
            _secretsManagerClient = null;
            return;
        }

        _config = config.Value.AWSSecretsManager;

        try
        {
            // Determine region - use configured region or default to us-east-1
            var region = RegionEndpoint.GetBySystemName(_config.Region ?? "us-east-1");

            // Configure credentials and create client
            // AmazonSecretsManagerClient uses the default credential chain if credentials are not provided:
            // 1. IAM role (when running on EC2/ECS/Lambda)
            // 2. Environment variables (AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY)
            // 3. Credentials file (~/.aws/credentials)
            // 4. IAM role for tasks (when using ECS task roles)
            if (!string.IsNullOrWhiteSpace(_config.AccessKeyId) &&
                !string.IsNullOrWhiteSpace(_config.SecretAccessKey))
            {
                // Use explicit access key credentials if provided
                _logger.LogInformation("Using explicit access key credentials for AWS Secrets Manager");
                var credentials = new BasicAWSCredentials(_config.AccessKeyId, _config.SecretAccessKey);
                _secretsManagerClient = new AmazonSecretsManagerClient(credentials, region);
            }
            else
            {
                // Use default credential chain (IAM role, environment variables, credentials file, etc.)
                _logger.LogInformation("Using default AWS credential chain for Secrets Manager");
                // Note: AssumeRole is not implemented here - would require STS client
                // For now, the default client will use the credential chain automatically
                _secretsManagerClient = new AmazonSecretsManagerClient(region);
            }
            _logger.LogInformation("AWS Secrets Manager client initialized for region: {Region}", region.SystemName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize AWS Secrets Manager client. Provider will not be available.");
            _secretsManagerClient = null;
        }
    }

    /// <summary>
    /// Retrieves a secret value from AWS Secrets Manager
    /// </summary>
    /// <param name="secretName">Secret name or ARN (e.g., "ai-openai-apikey" or "arn:aws:secretsmanager:us-east-1:123456789012:secret:ai-openai-apikey-AbCdEf")</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The secret value if found, null if secret does not exist</returns>
    public async Task<string?> GetSecretAsync(string secretName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(secretName))
        {
            _logger.LogWarning("Attempted to retrieve secret with empty or null name");
            return null;
        }

        if (_secretsManagerClient == null)
        {
            _logger.LogWarning("AWS Secrets Manager client is not configured. Cannot retrieve secret '{SecretName}'.", secretName);
            return null;
        }

        try
        {
            _logger.LogDebug("Retrieving secret '{SecretName}' from AWS Secrets Manager", secretName);

            var request = new GetSecretValueRequest
            {
                SecretId = secretName
            };

            var response = await _secretsManagerClient.GetSecretValueAsync(request, cancellationToken);

            if (response?.SecretString == null)
            {
                _logger.LogWarning("Secret '{SecretName}' not found or has no value in AWS Secrets Manager", secretName);
                return null;
            }

            _logger.LogDebug("Successfully retrieved secret '{SecretName}' from AWS Secrets Manager", secretName);
            return response.SecretString;
        }
        catch (ResourceNotFoundException ex)
        {
            // Secret not found - return null (graceful degradation)
            _logger.LogWarning("Secret '{SecretName}' not found in AWS Secrets Manager: {Message}",
                secretName, ex.Message);
            return null;
        }
        catch (InvalidRequestException ex)
        {
            // Invalid request (e.g., invalid ARN format)
            _logger.LogError(ex, "Invalid request for secret '{SecretName}' in AWS Secrets Manager: {Message}",
                secretName, ex.Message);
            return null;
        }
        catch (InvalidParameterException ex)
        {
            // Invalid parameter
            _logger.LogError(ex, "Invalid parameter for secret '{SecretName}' in AWS Secrets Manager: {Message}",
                secretName, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving secret '{SecretName}' from AWS Secrets Manager",
                secretName);
            return null;
        }
    }

    /// <summary>
    /// Retrieves multiple secrets in batch from AWS Secrets Manager
    /// </summary>
    /// <param name="secretNames">Collection of secret names or ARNs to retrieve</param>
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

        // AWS Secrets Manager doesn't have a batch API, so we retrieve secrets sequentially
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

        _logger.LogDebug("Retrieved {Count} of {Total} requested secrets from AWS Secrets Manager",
            result.Count, secretNames.Count());

        return result;
    }

    /// <summary>
    /// Checks if a secret exists in AWS Secrets Manager
    /// </summary>
    /// <param name="secretName">The name or ARN of the secret to check</param>
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
            _logger.LogWarning(ex, "Error checking existence of secret '{SecretName}' in AWS Secrets Manager", secretName);
            return false;
        }
    }
}
