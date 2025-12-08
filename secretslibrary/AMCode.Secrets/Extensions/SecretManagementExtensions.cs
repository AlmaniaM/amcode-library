using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AMCode.Secrets;
using AMCode.Secrets.Providers;

namespace AMCode.Secrets.Extensions
{
    /// <summary>
    /// Extension methods for registering secret management services
    /// </summary>
    public static class SecretManagementExtensions
    {
        /// <summary>
        /// Adds secret management services to the service collection
        /// Registers all providers and the facade that selects the active provider based on configuration
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddSecretManagement(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // Register configuration
            services.Configure<SecretManagementConfig>(configuration.GetSection("SecretManagement"));

            // Register all secret service providers
            // Each provider must be registered as both the concrete type AND ISecretService
            // This allows resolution by concrete type (for facade) and by interface (for collection resolution)
            
            // Environment Variable provider
            services.AddSingleton<EnvironmentVariableSecretService>();
            services.AddSingleton<ISecretService>(provider => provider.GetRequiredService<EnvironmentVariableSecretService>());

            // Azure Key Vault provider
            services.AddSingleton<AzureKeyVaultSecretService>();
            services.AddSingleton<ISecretService>(provider => provider.GetRequiredService<AzureKeyVaultSecretService>());

            // AWS Secrets Manager provider
            services.AddSingleton<AWSSecretsManagerService>();
            services.AddSingleton<ISecretService>(provider => provider.GetRequiredService<AWSSecretsManagerService>());

            // GCP Secret Manager provider
            services.AddSingleton<GCPSecretManagerService>();
            services.AddSingleton<ISecretService>(provider => provider.GetRequiredService<GCPSecretManagerService>());

            // Register the facade as the primary ISecretService implementation
            // This facade selects the appropriate provider based on configuration
            // Note: We must register providers first, then the facade, to avoid circular dependencies
            services.AddSingleton<ISecretService>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<SecretManagementConfig>>();
                var logger = provider.GetRequiredService<ILogger<SecretServiceFacade>>();

                // Get all registered ISecretService implementations EXCEPT the facade
                // Since we're in the factory method creating the facade, we need to manually
                // collect the individual providers rather than using GetServices which would
                // try to resolve the facade itself
                var providers = new List<ISecretService>
                {
                    provider.GetRequiredService<EnvironmentVariableSecretService>(),
                    provider.GetRequiredService<AzureKeyVaultSecretService>(),
                    provider.GetRequiredService<AWSSecretsManagerService>(),
                    provider.GetRequiredService<GCPSecretManagerService>()
                };

                return new SecretServiceFacade(config, providers, logger);
            });

            return services;
        }
    }
}

