# AMCode.Secrets

**Version**: 1.0.0  
**Target Framework**: .NET 9.0  
**Purpose**: Unified secret management system supporting multiple cloud providers and local development

---

## Overview

AMCode.Secrets provides a unified abstraction for accessing secrets across multiple cloud providers (Azure Key Vault, AWS Secrets Manager, GCP Secret Manager) and local development environments (environment variables). The library implements the Facade pattern to select the appropriate secret provider based on configuration, allowing seamless switching between providers without code changes.

## Components

### Interfaces

- **ISecretService**: Core interface for secret management operations
  - `GetSecretAsync(string secretName, CancellationToken)` - Retrieve a single secret
  - `GetSecretsAsync(IEnumerable<string> secretNames, CancellationToken)` - Retrieve multiple secrets
  - `SecretExistsAsync(string secretName, CancellationToken)` - Check if a secret exists

### Configuration

- **SecretManagementConfig**: Main configuration class
  - `Provider`: The secret provider to use (Environment, AzureKeyVault, AWSSecretsManager, GCPSecretManager)
  - `AzureKeyVault`: Azure Key Vault configuration
  - `AWSSecretsManager`: AWS Secrets Manager configuration
  - `GCPSecretManager`: GCP Secret Manager configuration

- **SecretProvider**: Enumeration of available providers
  - `Environment`: Environment variables (via ASP.NET Core IConfiguration)
  - `AzureKeyVault`: Azure Key Vault
  - `AWSSecretsManager`: AWS Secrets Manager
  - `GCPSecretManager`: GCP Secret Manager

- **Provider-Specific Configurations**:
  - `AzureKeyVaultConfig`: Azure Key Vault settings (VaultUri, TenantId, ClientId, ClientSecret)
  - `AWSSecretsManagerConfig`: AWS settings (Region, AccessKeyId, SecretAccessKey, RoleArn)
  - `GCPSecretManagerConfig`: GCP settings (ProjectId, CredentialsPath)

### Extensions

- **SecretManagementExtensions**: Dependency injection extension methods
  - `AddSecretManagement(IServiceCollection, IConfiguration)` - Register secret management services

## Dependencies

- `AMCode.Common`: Common utilities and models
- `Azure.Security.KeyVault.Secrets`: Azure Key Vault SDK
- `AWSSDK.SecretsManager`: AWS Secrets Manager SDK
- `Google.Cloud.SecretManager.V1`: GCP Secret Manager SDK
- `Microsoft.Extensions.Configuration.Abstractions`: Configuration abstractions
- `Microsoft.Extensions.DependencyInjection.Abstractions`: DI abstractions
- `Microsoft.Extensions.Options`: Options pattern
- `Microsoft.Extensions.Logging.Abstractions`: Logging abstractions

## Usage

### Configuration

Add to `appsettings.json`:

```json
{
  "SecretManagement": {
    "Provider": "Environment",
    "AzureKeyVault": {
      "VaultUri": "https://your-vault.vault.azure.net/"
    },
    "AWSSecretsManager": {
      "Region": "us-east-1"
    },
    "GCPSecretManager": {
      "ProjectId": "your-project-id"
    }
  }
}
```

### Dependency Injection

```csharp
using AMCode.Secrets.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register secret management
builder.Services.AddSecretManagement(builder.Configuration);
```

### Using ISecretService

```csharp
using AMCode.Secrets;

public class MyService
{
    private readonly ISecretService _secretService;

    public MyService(ISecretService secretService)
    {
        _secretService = secretService;
    }

    public async Task<string?> GetApiKeyAsync()
    {
        return await _secretService.GetSecretAsync("api-key", cancellationToken);
    }

    public async Task<Dictionary<string, string>> GetMultipleSecretsAsync()
    {
        var secretNames = new[] { "api-key", "database-password" };
        return await _secretService.GetSecretsAsync(secretNames, cancellationToken);
    }
}
```

## Provider Implementations

The library includes the following provider implementations:

- **EnvironmentVariableSecretService**: Environment variable provider (local development)
  - Reads from User Secrets, environment variables, and appsettings.json
  - Located in `Providers/EnvironmentVariableSecretService.cs`

- **AzureKeyVaultSecretService**: Azure Key Vault provider
  - Uses DefaultAzureCredential for authentication
  - Supports Managed Identity, Azure CLI, Visual Studio credentials
  - Located in `Providers/AzureKeyVaultSecretService.cs`

- **AWSSecretsManagerService**: AWS Secrets Manager provider
  - Uses IAM roles (ECS/Lambda) or access keys for authentication
  - Located in `Providers/AWSSecretsManagerService.cs`

- **GCPSecretManagerService**: GCP Secret Manager provider
  - Uses Application Default Credentials (Cloud Run) or service account JSON
  - Located in `Providers/GCPSecretManagerService.cs`

- **SecretServiceFacade**: Facade that selects the appropriate provider based on configuration
  - Automatically selects provider based on `SecretManagementConfig.Provider`
  - Located in `SecretServiceFacade.cs`

## Configuration Provider

- **SecretConfigurationProvider**: Loads secrets from ISecretService into IConfiguration
  - Allows existing code using IConfiguration to work with the secret management system
  - Located in `SecretConfigurationProvider.cs`
  - Note: Contains application-specific secret mappings. For a generic library, these mappings should be made configurable.

- **SecretConfigurationSource**: Configuration source for the secret configuration provider
  - Located in `SecretConfigurationSource.cs`
  - Use `AddSecretManagementConfiguration()` extension method to add to IConfigurationBuilder

## Testing

Unit tests are located in `AMCode.Secrets.UnitTests` project.

## Notes

- All provider implementations are included and ready to use
- The library has zero dependencies on RecipeOCR-specific code
- All components are generic and reusable across any .NET application
- `SecretConfigurationProvider` has no default mappings - applications should extend it to provide their own configuration-to-secret mappings
