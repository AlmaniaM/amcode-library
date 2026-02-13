# AMCode.Secrets — Agent Guide

## What This Is

Multi-cloud secret management abstraction (.NET 9) supporting Azure Key Vault, AWS Secrets Manager, GCP Secret Manager, and environment variables.

## When to Use

- Retrieve API keys, connection strings, or credentials at runtime
- Need provider-agnostic secret access
- Secret rotation or vault management

## When NOT to Use

- .NET user-secrets for local dev → use `dotnet user-secrets` directly
- Hardcoded config values → use `appsettings.json`
- Application config (non-secret) → use `IConfiguration`

## Key Interfaces

| Interface | Purpose |
|-----------|---------|
| `ISecretService` | GetSecretAsync, GetSecretsAsync, SecretExistsAsync |
| `SecretServiceFacade` | Unified facade for multiple providers |

## Providers

- `AzureKeyVaultSecretService` — Azure Key Vault
- `AWSSecretsManagerService` — AWS Secrets Manager
- `GCPSecretManagerService` — GCP Secret Manager
- `EnvironmentVariableSecretService` — Environment variables (fallback)

## DI Registration

```csharp
services.AddSecretManagement(configuration);
```

## Verification

```bash
cd repos/amcode-library
dotnet build secretslibrary/AMCode.Secrets/AMCode.Secrets.csproj
```
