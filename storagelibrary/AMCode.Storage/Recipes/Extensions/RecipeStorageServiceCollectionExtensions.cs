using AMCode.Storage.Recipes.Interfaces;
using AMCode.Storage.Recipes;
using AMCode.Storage.Recipes.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AMCode.Storage.Recipes.Extensions
{
    /// <summary>
    /// Extension methods for adding recipe storage services to the DI container
    /// </summary>
    public static class RecipeStorageServiceCollectionExtensions
    {
        /// <summary>
        /// Adds recipe storage services to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddRecipeStorageServices(this IServiceCollection services)
        {
            // Register interfaces
            services.AddScoped<IRecipeImageStorageService, RecipeImageStorageService>();
            
            return services;
        }
        
        /// <summary>
        /// Adds recipe storage services with custom configuration
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configureOptions">Action to configure recipe storage options</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddRecipeStorageServices(
            this IServiceCollection services,
            System.Action<RecipeImageStorageOptions> configureOptions)
        {
            // Configure options
            services.Configure(configureOptions);
            
            // Register services
            services.AddRecipeStorageServices();
            
            return services;
        }
    }
}
