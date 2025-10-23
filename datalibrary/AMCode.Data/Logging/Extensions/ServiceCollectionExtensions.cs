using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AMCode.Data.Logging.Configuration;
using AMCode.Data.Logging.Infrastructure;
using AMCode.Data.Logging.Infrastructure.Console;

namespace AMCode.Data.Logging.Extensions
{
    /// <summary>
    /// Service collection extensions for AMCode.Data logging registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAMCodeDataLogging(this IServiceCollection services, IConfiguration configuration = null)
        {
            // Register configuration
            if (configuration != null)
            {
                services.Configure<LoggingConfiguration>(configuration.GetSection("AMCodeData:Logging"));
            }
            else
            {
                services.Configure<LoggingConfiguration>(_ => new LoggingConfiguration());
            }

            // Register core services
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton<ILoggerProvider, NoOpLoggerProvider>(); // Default to no-op

            // Register typed loggers
            services.AddScoped(typeof(ILogger<>), typeof(Logger<>));

            return services;
        }

        public static IServiceCollection AddAMCodeDataConsoleLogging(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerProvider, ConsoleLoggerProvider>();
            return services;
        }
    }
}
