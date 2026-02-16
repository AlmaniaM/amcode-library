using AMCode.Documents.TextExtraction.Domain.Interfaces;
using AMCode.Documents.TextExtraction.Providers.OpenXml;
using AMCode.Documents.TextExtraction.Providers.PdfPig;
using Microsoft.Extensions.DependencyInjection;

namespace AMCode.Documents.TextExtraction.Extensions
{
    /// <summary>
    /// Service collection extensions for document text extraction registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register document text extraction services with default providers
        /// (PdfPig for PDF, OpenXml for DOCX)
        /// </summary>
        public static IServiceCollection AddDocumentTextExtraction(this IServiceCollection services)
        {
            services.AddSingleton<IPdfTextExtractor, PdfPigTextExtractor>();
            services.AddSingleton<IDocxTextExtractor, OpenXmlDocxTextExtractor>();
            services.AddSingleton<IDocumentTextExtractor, DocumentTextExtractor>();
            return services;
        }
    }
}
