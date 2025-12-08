using AMCode.Web.Behaviors;
using AMCode.Web.Middleware;
using AMCode.Web.Serialization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AMCode.Web.Extensions
{
    /// <summary>
    /// Extension methods for registering AMCode.Web services and middleware
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers AMCode.Web middleware in the application pipeline
        /// </summary>
        /// <param name="app">The application builder</param>
        /// <returns>The application builder for chaining</returns>
        public static IApplicationBuilder UseAMCodeWeb(this IApplicationBuilder app)
        {
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }

        /// <summary>
        /// Registers AMCode.Web MediatR behaviors
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddAMCodeWebBehaviors(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            return services;
        }

        /// <summary>
        /// Configures JSON serialization with flexible camelCase naming policy for MVC controllers
        /// </summary>
        /// <param name="builder">The MVC builder</param>
        /// <returns>The MVC builder for chaining</returns>
        public static IMvcBuilder AddAMCodeWebJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = new FlexibleCamelCaseNamingPolicy();
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
            return builder;
        }

        /// <summary>
        /// Registers all AMCode.Web services (behaviors)
        /// Note: JSON options should be configured via AddAMCodeWebJsonOptions on IMvcBuilder
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddAMCodeWeb(this IServiceCollection services)
        {
            services.AddAMCodeWebBehaviors();
            return services;
        }
    }
}
