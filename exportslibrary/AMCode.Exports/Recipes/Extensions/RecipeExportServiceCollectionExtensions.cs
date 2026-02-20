using AMCode.Exports.Recipes.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AMCode.Exports.Recipes.Extensions
{
    /// <summary>
    /// Extension methods for adding recipe export services to the DI container.
    /// </summary>
    public static class RecipeExportServiceCollectionExtensions
    {
        /// <summary>
        /// Adds recipe export services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddRecipeExportServices(this IServiceCollection services)
        {
            services.AddScoped<IRecipeExportBuilder, RecipeExportBuilder>();
            services.AddTransient<IRecipeExcelBuilder, RecipeExcelBuilder>();
            services.AddTransient<IRecipeCsvBuilder, RecipeCsvBuilder>();
            services.AddTransient<BulkRecipeExporter>();

            return services;
        }

        /// <summary>
        /// Adds recipe export services with custom configuration.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureOptions">Action to configure recipe export options.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddRecipeExportServices(
            this IServiceCollection services,
            System.Action<Models.RecipeExportOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddRecipeExportServices();

            return services;
        }
    }
}
