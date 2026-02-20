using AMCode.Storage.ImageUtilities;
using Microsoft.Extensions.DependencyInjection;

namespace AMCode.Storage.Extensions
{
    /// <summary>
    /// Extension methods for registering image utility services with the DI container
    /// </summary>
    public static class ImageUtilityServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="IImageUtility"/> (as <see cref="ImageSharpImageUtility"/>) as a singleton.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddImageUtilities(this IServiceCollection services)
        {
            services.AddSingleton<IImageUtility, ImageSharpImageUtility>();
            return services;
        }
    }
}
