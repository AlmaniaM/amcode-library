namespace AMCode.Secrets;

/// <summary>
/// Configuration for secret management system
/// Supports multiple cloud providers and environment variables for local development
/// </summary>
public class SecretManagementConfig
{
    /// <summary>
    /// The secret provider to use (Environment, AzureKeyVault, AWSSecretsManager, GCPSecretManager)
    /// Default: Environment for local development
    /// </summary>
    public SecretProvider Provider { get; set; } = SecretProvider.Environment;

    /// <summary>
    /// Azure Key Vault configuration
    /// </summary>
    public AzureKeyVaultConfig AzureKeyVault { get; set; } = new();

    /// <summary>
    /// AWS Secrets Manager configuration
    /// </summary>
    public AWSSecretsManagerConfig AWSSecretsManager { get; set; } = new();

    /// <summary>
    /// GCP Secret Manager configuration
    /// </summary>
    public GCPSecretManagerConfig GCPSecretManager { get; set; } = new();
}

/// <summary>
/// Available secret management providers
/// </summary>
public enum SecretProvider
{
    /// <summary>
    /// Environment variables (via ASP.NET Core IConfiguration)
    /// Works identically across Azure App Service, AWS Lambda/ECS, GCP Cloud Run
    /// </summary>
    Environment,

    /// <summary>
    /// Azure Key Vault
    /// Uses DefaultAzureCredential for authentication (Managed Identity, Azure CLI, Visual Studio, etc.)
    /// </summary>
    AzureKeyVault,

    /// <summary>
    /// AWS Secrets Manager
    /// Uses IAM roles (ECS/Lambda) or access keys for authentication
    /// </summary>
    AWSSecretsManager,

    /// <summary>
    /// GCP Secret Manager
    /// Uses Application Default Credentials (Cloud Run) or service account JSON
    /// </summary>
    GCPSecretManager
}

/// <summary>
/// Azure Key Vault configuration
/// </summary>
public class AzureKeyVaultConfig
{
    /// <summary>
    /// Azure Key Vault URI (e.g., https://your-vault.vault.azure.net/)
    /// </summary>
    public string VaultUri { get; set; } = string.Empty;

    /// <summary>
    /// Azure AD Tenant ID (optional, uses DefaultAzureCredential if not specified)
    /// </summary>
    public string TenantId { get; set; } = string.Empty;

    /// <summary>
    /// Azure AD Client ID (optional, uses DefaultAzureCredential if not specified)
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Azure AD Client Secret (optional, uses DefaultAzureCredential if not specified)
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
}

/// <summary>
/// AWS Secrets Manager configuration
/// </summary>
public class AWSSecretsManagerConfig
{
    /// <summary>
    /// AWS Region (e.g., us-east-1)
    /// </summary>
    public string Region { get; set; } = "us-east-1";

    /// <summary>
    /// AWS Access Key ID (optional, uses IAM role if not specified)
    /// </summary>
    public string AccessKeyId { get; set; } = string.Empty;

    /// <summary>
    /// AWS Secret Access Key (optional, uses IAM role if not specified)
    /// </summary>
    public string SecretAccessKey { get; set; } = string.Empty;

    /// <summary>
    /// IAM Role ARN for assume role (optional)
    /// </summary>
    public string RoleArn { get; set; } = string.Empty;
}

/// <summary>
/// GCP Secret Manager configuration
/// </summary>
public class GCPSecretManagerConfig
{
    /// <summary>
    /// GCP Project ID
    /// </summary>
    public string ProjectId { get; set; } = string.Empty;

    /// <summary>
    /// Path to service account JSON credentials file (optional, uses Application Default Credentials if not specified)
    /// </summary>
    public string CredentialsPath { get; set; } = string.Empty;
}
