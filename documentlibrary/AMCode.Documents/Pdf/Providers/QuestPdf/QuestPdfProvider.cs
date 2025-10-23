using System;
using System.IO;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// QuestPDF provider implementation
    /// </summary>
    public class QuestPdfProvider : IPdfProvider
    {
        public string Name => "QuestPDF";
        public string Version => "2024.12.4";

        private readonly IPdfLogger _logger;
        private readonly IPdfValidator _validator;

        /// <summary>
        /// Create QuestPDF provider
        /// </summary>
        public QuestPdfProvider(IPdfLogger logger, IPdfValidator validator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Create a new PDF document
        /// </summary>
        public Result<IPdfDocument> CreateDocument()
        {
            try
            {
                _logger.LogDocumentOperation("CreateDocument");
                var engine = new QuestPdfEngine(_logger, _validator);
                return engine.CreateDocument();
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateDocument", ex);
                return Result<IPdfDocument>.Failure(ex.Message, ex);
            }
        }

        /// <summary>
        /// Create a new PDF document with properties
        /// </summary>
        public Result<IPdfDocument> CreateDocument(IPdfProperties properties)
        {
            try
            {
                _logger.LogDocumentOperation("CreateDocument", properties);
                var engine = new QuestPdfEngine(_logger, _validator);
                return engine.CreateDocument(properties);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateDocument", ex);
                return Result<IPdfDocument>.Failure(ex.Message, ex);
            }
        }

        /// <summary>
        /// Open existing PDF document from stream
        /// </summary>
        public Result<IPdfDocument> OpenDocument(Stream stream)
        {
            try
            {
                _logger.LogDocumentOperation("OpenDocument");
                var engine = new QuestPdfEngine(_logger, _validator);
                return engine.OpenDocument(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError("OpenDocument", ex);
                return Result<IPdfDocument>.Failure(ex.Message, ex);
            }
        }

        /// <summary>
        /// Open existing PDF document from file
        /// </summary>
        public Result<IPdfDocument> OpenDocument(string filePath)
        {
            try
            {
                _logger.LogDocumentOperation("OpenDocument", new { filePath });
                var engine = new QuestPdfEngine(_logger, _validator);
                return engine.OpenDocument(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError("OpenDocument", ex);
                return Result<IPdfDocument>.Failure(ex.Message, ex);
            }
        }
    }
}
