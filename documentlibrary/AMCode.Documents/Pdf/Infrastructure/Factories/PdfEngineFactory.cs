using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF engine factory
    /// </summary>
    public class PdfEngineFactory
    {
        private readonly IPdfLogger _logger;
        private readonly IPdfValidator _validator;

        /// <summary>
        /// Create PDF engine factory
        /// </summary>
        public PdfEngineFactory(IPdfLogger logger = null, IPdfValidator validator = null)
        {
            _logger = logger ?? new PdfLogger();
            _validator = validator ?? new PdfValidator();
        }

        /// <summary>
        /// Create PDF engine for QuestPDF
        /// </summary>
        public Result<IPdfEngine> CreateQuestPdfEngine()
        {
            try
            {
                var engine = new QuestPdfEngine(_logger, _validator);
                _logger.LogInformation("Created QuestPDF engine");
                return Result<IPdfEngine>.Success(engine);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateQuestPdfEngine", ex);
                return Result<IPdfEngine>.Failure(ex.Message, ex);
            }
        }

        /// <summary>
        /// Create PDF engine for iTextSharp
        /// </summary>
        public Result<IPdfEngine> CreateiTextSharpEngine()
        {
            try
            {
                var engine = new iTextSharpEngine(_logger, _validator);
                _logger.LogInformation("Created iTextSharp engine");
                return Result<IPdfEngine>.Success(engine);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateiTextSharpEngine", ex);
                return Result<IPdfEngine>.Failure(ex.Message, ex);
            }
        }

        /// <summary>
        /// Create PDF engine by provider name
        /// </summary>
        public Result<IPdfEngine> CreateEngine(string providerName)
        {
            if (string.IsNullOrWhiteSpace(providerName))
                return Result<IPdfEngine>.Failure("Provider name cannot be null or empty");

            return providerName.ToLowerInvariant() switch
            {
                "questpdf" => CreateQuestPdfEngine(),
                "itextsharp" => CreateiTextSharpEngine(),
                _ => Result<IPdfEngine>.Failure($"Unknown provider: {providerName}")
            };
        }
    }
}
