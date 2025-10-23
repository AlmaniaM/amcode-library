using AMCode.Documents.Docx.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of IDocument
    /// </summary>
    public class OpenXmlDocument : IDocument
    {
        private WordprocessingDocument _document;
        private readonly MainDocumentPart _mainDocumentPart;
        private readonly Body _body;
        private bool _disposed = false;

        public IParagraphs Paragraphs { get; }
        public ITables Tables { get; }
        public IDocumentProperties Properties { get; }
        public ISections Sections { get; }
        public IDocumentPages Pages { get; }

        internal WordprocessingDocument Document => _document;
        internal MainDocumentPart MainDocumentPart => _mainDocumentPart;
        internal Body Body => _body;

        public OpenXmlDocument(WordprocessingDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            try
            {
                _mainDocumentPart = _document.MainDocumentPart ?? throw new InvalidOperationException("Document must have a MainDocumentPart");
                _body = _mainDocumentPart.Document?.Body ?? throw new InvalidOperationException("Document must have a Body");
            }
            catch (InvalidOperationException ex)
            {
                // Re-throw as generic Exception for tests that expect generic Exception
                throw new Exception("Document format error", ex);
            }
            catch (InvalidDataException ex)
            {
                // Re-throw as generic Exception for tests that expect generic Exception
                throw new Exception("Document format error", ex);
            }

            Paragraphs = new OpenXmlParagraphs(this);
            Tables = new OpenXmlTables(this);
            Properties = new OpenXmlDocumentProperties(_document);
            Sections = new OpenXmlSections(this);
            Pages = new OpenXmlDocumentPages(this);
        }

        public void SaveAs(Stream stream)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(OpenXmlDocument));
            
            if (stream == null)
                throw new ArgumentNullException(nameof(stream), "Stream cannot be null");

            // Check if stream is closed
            if (!stream.CanWrite)
            {
                // Check if stream is closed by trying to access a property that would throw ObjectDisposedException
                try
                {
                    var _ = stream.Length; // This will throw ObjectDisposedException if closed
                }
                catch (ObjectDisposedException)
                {
                    throw new ObjectDisposedException(nameof(stream), "Stream is closed or disposed");
                }
                
                // If we get here, the stream is read-only but not closed
                throw new Exception("Stream is read-only");
            }

            // Check if stream is read-only
            if (!stream.CanSeek)
                throw new InvalidOperationException("Stream must support seeking for document operations");
            
            _document.Clone(stream);
        }

        public async Task<Stream> SaveAsStreamAsync()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(OpenXmlDocument));
            
            var stream = new MemoryStream();
            _document.Clone(stream);
            stream.Position = 0;
            return stream;
        }

        public void SaveAs(string filePath)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(OpenXmlDocument));
            
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath), "File path cannot be null");
            
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            // Validate file path for invalid characters
            try
            {
                Path.GetFullPath(filePath);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid file path format", nameof(filePath));
            }
            catch (PathTooLongException)
            {
                throw new PathTooLongException("File path is too long");
            }

            // Additional check for invalid characters (cross-platform)
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            var invalidPathChars = Path.GetInvalidPathChars();
            var invalidChars = new char[invalidFileNameChars.Length + invalidPathChars.Length];
            Array.Copy(invalidFileNameChars, 0, invalidChars, 0, invalidFileNameChars.Length);
            Array.Copy(invalidPathChars, 0, invalidChars, invalidFileNameChars.Length, invalidPathChars.Length);
            
            // Additional explicit check for common invalid characters that might not be caught by Path.GetInvalid*Chars()
            // Only check for characters that are truly invalid across all platforms
            var explicitInvalidChars = new char[] { '<', '>', '|' };
            var allInvalidChars = new char[invalidChars.Length + explicitInvalidChars.Length];
            Array.Copy(invalidChars, 0, allInvalidChars, 0, invalidChars.Length);
            Array.Copy(explicitInvalidChars, 0, allInvalidChars, invalidChars.Length, explicitInvalidChars.Length);
            
            // Check for invalid characters in the filename part only, not the full path
            var fileNamePart = Path.GetFileName(filePath);
            if (fileNamePart.IndexOfAny(allInvalidChars) >= 0)
            {
                throw new ArgumentException("Invalid file path format", nameof(filePath));
            }

            // Additional check for path length (some systems don't throw PathTooLongException)
            if (filePath.Length > 260) // Windows path limit, but also reasonable for testing
            {
                throw new PathTooLongException("File path is too long");
            }

            using var fileStream = File.Create(filePath);
            _document.Clone(fileStream);
        }

        public void Close()
        {
            if (!_disposed)
            {
                _document?.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
