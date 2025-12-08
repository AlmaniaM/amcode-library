namespace AMCode.Secrets;

/// <summary>
/// Service interface for secret management operations
/// Provides unified abstraction for accessing secrets across multiple cloud providers and local development environments
/// </summary>
public interface ISecretService
{
    /// <summary>
    /// Retrieves a secret value by name
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve (format depends on provider: kebab-case for cloud, double-underscore for environment variables)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The secret value if found, null if secret does not exist</returns>
    Task<string?> GetSecretAsync(string secretName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves multiple secrets by name in a single operation
    /// </summary>
    /// <param name="secretNames">Collection of secret names to retrieve</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dictionary of secret names to their values. Missing secrets are not included in the dictionary</returns>
    Task<Dictionary<string, string>> GetSecretsAsync(IEnumerable<string> secretNames, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a secret exists
    /// </summary>
    /// <param name="secretName">The name of the secret to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the secret exists, false otherwise</returns>
    Task<bool> SecretExistsAsync(string secretName, CancellationToken cancellationToken = default);
}

