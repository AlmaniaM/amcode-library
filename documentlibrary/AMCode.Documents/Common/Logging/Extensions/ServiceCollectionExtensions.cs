using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AMCode.Documents.Common.Logging;
using AMCode.Documents.Common.Logging.Configuration;
using AMCode.Documents.Common.Logging.Infrastructure;
using AMCode.Documents.Pdf;
using AMCode.Documents.Pdf.Infrastructure;
using AMCode.Documents.Excel.Infrastructure;
using AMCode.Documents.Docx.Infrastructure;
using AMCode.Docx.Infrastructure.Interfaces;

namespace AMCode.Documents.Common.Logging.Extensions
{
    /// <summary>
    /// Service collection extensions for document logging registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds document logging services to the service collection
        /// </summary>
        public static IServiceCollection AddDocumentLogging(this IServiceCollection services, IConfiguration configuration)
        {
                   // Register configuration (simplified for now)
                   services.Configure<DocumentLoggingConfiguration>(options => { });
                   services.Configure<ConsoleDocumentLoggerConfiguration>(options => { });
                   services.Configure<FileDocumentLoggerConfiguration>(options => { });
                   services.Configure<StructuredDocumentLoggerConfiguration>(options => { });

            // Register core services
            services.AddSingleton<IDocumentLoggerFactory, DocumentLoggerFactory>();
            services.AddSingleton<IDocumentLoggerProvider, CompositeDocumentLoggerProvider>();
            services.AddSingleton<DocumentLogFileManager>();
            services.AddSingleton<DocumentLogFormatter>();

            // Register specific logger providers
            services.AddSingleton<IDocumentLoggerProvider, ConsoleDocumentLoggerProvider>();
            services.AddSingleton<IDocumentLoggerProvider, FileDocumentLoggerProvider>();
            services.AddSingleton<IDocumentLoggerProvider, StructuredDocumentLoggerProvider>();

            // Register document-specific loggers
            services.AddScoped<IPdfLogger, PdfDocumentLogger>();
            services.AddScoped<IExcelLogger, ExcelDocumentLogger>();
            services.AddScoped<AMCode.Docx.Infrastructure.Interfaces.IDocumentLogger, DocxDocumentLogger>();

            return services;
        }

        /// <summary>
        /// Adds document logging services with context support
        /// </summary>
        public static IServiceCollection AddDocumentLoggingWithContext(this IServiceCollection services)
        {
            services.AddScoped<IDocumentLoggerContext, DocumentLoggerContext>();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
            return services;
        }

        /// <summary>
        /// Adds document logging services with default configuration
        /// </summary>
        public static IServiceCollection AddDocumentLogging(this IServiceCollection services)
        {
            var configuration = new DocumentLoggingConfiguration();
            var provider = new CompositeDocumentLoggerProvider(configuration);
            var consoleConfig = new ConsoleDocumentLoggerConfiguration();
            
            // Add console provider
            provider.AddProvider(new ConsoleDocumentLoggerProvider(configuration, consoleConfig));

            // Register core services
            services.AddSingleton<IDocumentLoggerFactory>(sp => new DocumentLoggerFactory(provider, configuration));
            services.AddSingleton<IDocumentLoggerProvider>(provider);

            // Register document-specific loggers
            services.AddScoped<IPdfLogger>(sp => new PdfDocumentLogger("AMCode.Pdf", provider, configuration));
            services.AddScoped<IExcelLogger>(sp => new ExcelDocumentLogger("AMCode.Excel", provider, configuration));
            services.AddScoped<AMCode.Docx.Infrastructure.Interfaces.IDocumentLogger>(sp => new DocxDocumentLogger("AMCode.Docx", provider, configuration));

            return services;
        }
    }
}
