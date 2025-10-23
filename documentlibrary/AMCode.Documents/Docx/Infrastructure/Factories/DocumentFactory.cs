using AMCode.Documents.Docx.Interfaces;
using System;
using System.IO;
using AMCode.Docx;
using AMCode.Documents.Common.Models;
using AMCode.Docx.Domain.Models;
using AMCode.Docx.Infrastructure.Adapters;
using AMCode.Docx.Infrastructure.Interfaces;
using AMCode.Docx.Providers.OpenXml;

namespace AMCode.Docx.Infrastructure.Factories
{
    /// <summary>
    /// Infrastructure implementation of document factory with dependency injection
    /// </summary>
    public class DocumentFactory : IDocumentFactory
    {
        private readonly IWordprocessingDocumentFactory _documentFactory;
        private readonly IDocumentLogger _logger;
        private readonly IDocumentValidator _validator;

        public DocumentFactory(
            IWordprocessingDocumentFactory documentFactory,
            IDocumentLogger logger,
            IDocumentValidator validator)
        {
            _documentFactory = documentFactory ?? throw new ArgumentNullException(nameof(documentFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public Result<IDocumentDomain> CreateDocument()
        {
            try
            {
                _logger.LogDocumentOperation("CreateDocument", null);

                var documentResult = _documentFactory.Create();
                if (!documentResult.IsSuccess)
                    return Result<IDocumentDomain>.Failure(documentResult.Error, documentResult.Exception);

                var document = new OpenXmlDocument(documentResult.Value);
                var contentAdapter = new DocumentContentAdapter(document);
                var metadataAdapter = new DocumentMetadataAdapter(document);
                var domainDocument = new DocumentDomain(contentAdapter, metadataAdapter);

                var validation = _validator.ValidateDocument(domainDocument);
                if (!validation.IsSuccess)
                {
                    _logger.LogError("CreateDocument", new InvalidOperationException(validation.Error));
                    return Result<IDocumentDomain>.Failure(validation.Error);
                }

                _logger.LogDocumentOperation("CreateDocument", new { DocumentId = domainDocument.Id });
                return Result<IDocumentDomain>.Success(domainDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateDocument", ex);
                return Result<IDocumentDomain>.Failure("Failed to create document", ex);
            }
        }

        public Result<IDocumentDomain> CreateDocument(string title, string content)
        {
            try
            {
                _logger.LogDocumentOperation("CreateDocument", new { Title = title, ContentLength = content?.Length ?? 0 });

                var documentResult = CreateDocument();
                if (!documentResult.IsSuccess)
                    return documentResult;

                var document = documentResult.Value;
                document.Properties.Title = title;

                var paragraph = document.Paragraphs.Create();
                paragraph.Text = content;

                var validation = _validator.ValidateDocument(document);
                if (!validation.IsSuccess)
                {
                    _logger.LogError("CreateDocument", new InvalidOperationException(validation.Error));
                    return Result<IDocumentDomain>.Failure(validation.Error);
                }

                _logger.LogDocumentOperation("CreateDocument", new { DocumentId = document.Id, Title = title });
                return Result<IDocumentDomain>.Success(document);
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateDocument", ex);
                return Result<IDocumentDomain>.Failure("Failed to create document with content", ex);
            }
        }

        public Result<IDocumentDomain> OpenDocument(Stream stream)
        {
            try
            {
                _logger.LogDocumentOperation("OpenDocument", new { StreamLength = stream?.Length ?? 0 });

                var documentResult = _documentFactory.Open(stream);
                if (!documentResult.IsSuccess)
                    return Result<IDocumentDomain>.Failure(documentResult.Error, documentResult.Exception);

                var document = new OpenXmlDocument(documentResult.Value);
                var contentAdapter = new DocumentContentAdapter(document);
                var metadataAdapter = new DocumentMetadataAdapter(document);
                var domainDocument = new DocumentDomain(contentAdapter, metadataAdapter);

                var validation = _validator.ValidateDocument(domainDocument);
                if (!validation.IsSuccess)
                {
                    _logger.LogError("OpenDocument", new InvalidOperationException(validation.Error));
                    return Result<IDocumentDomain>.Failure(validation.Error);
                }

                _logger.LogDocumentOperation("OpenDocument", new { DocumentId = domainDocument.Id });
                return Result<IDocumentDomain>.Success(domainDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError("OpenDocument", ex);
                return Result<IDocumentDomain>.Failure("Failed to open document from stream", ex);
            }
        }

        public Result<IDocumentDomain> OpenDocument(string filePath)
        {
            try
            {
                _logger.LogDocumentOperation("OpenDocument", new { FilePath = filePath });

                if (string.IsNullOrWhiteSpace(filePath))
                    return Result<IDocumentDomain>.Failure("File path cannot be null or empty");

                if (!File.Exists(filePath))
                    return Result<IDocumentDomain>.Failure($"File does not exist: {filePath}");

                var documentResult = _documentFactory.Open(filePath);
                if (!documentResult.IsSuccess)
                    return Result<IDocumentDomain>.Failure(documentResult.Error, documentResult.Exception);

                var document = new OpenXmlDocument(documentResult.Value);
                var contentAdapter = new DocumentContentAdapter(document);
                var metadataAdapter = new DocumentMetadataAdapter(document);
                var domainDocument = new DocumentDomain(contentAdapter, metadataAdapter);

                var validation = _validator.ValidateDocument(domainDocument);
                if (!validation.IsSuccess)
                {
                    _logger.LogError("OpenDocument", new InvalidOperationException(validation.Error));
                    return Result<IDocumentDomain>.Failure(validation.Error);
                }

                _logger.LogDocumentOperation("OpenDocument", new { DocumentId = domainDocument.Id, FilePath = filePath });
                return Result<IDocumentDomain>.Success(domainDocument);
            }
            catch (Exception ex)
            {
                _logger.LogError("OpenDocument", ex);
                return Result<IDocumentDomain>.Failure("Failed to open document from file", ex);
            }
        }
    }
}
