using System;
using System.IO;
using NUnit.Framework;
using AMCode.Docx;
using AMCode.Documents.Pdf;

namespace AMCode.Documents.UnitTests.ErrorHandling
{
    /// <summary>
    /// Tests for invalid format handling in the document library
    /// </summary>
    [TestFixture]
    public class FormatErrorTests
    {
        private string _tempDirectory;

        [SetUp]
        public void SetUp()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), "AMCodeDocumentsFormatTest", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (Directory.Exists(_tempDirectory))
                {
                    Directory.Delete(_tempDirectory, true);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        #region Invalid File Format Tests

        [Test]
        public void DocumentFactory_OpenDocument_WithInvalidDocxFormat_ShouldThrowException()
        {
            // Arrange
            var invalidDocxFile = Path.Combine(_tempDirectory, "Invalid.docx");
            File.WriteAllText(invalidDocxFile, "This is not a valid DOCX file");

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(invalidDocxFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithPdfAsDocx_ShouldThrowException()
        {
            // Arrange
            var pdfAsDocxFile = Path.Combine(_tempDirectory, "Document.docx");
            File.WriteAllBytes(pdfAsDocxFile, new byte[] { 0x25, 0x50, 0x44, 0x46 }); // PDF header

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(pdfAsDocxFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithExcelAsDocx_ShouldThrowException()
        {
            // Arrange
            var excelAsDocxFile = Path.Combine(_tempDirectory, "Document.docx");
            File.WriteAllBytes(excelAsDocxFile, new byte[] { 0x50, 0x4B, 0x03, 0x04 }); // ZIP header (Excel is ZIP-based)

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(excelAsDocxFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithTextAsDocx_ShouldThrowException()
        {
            // Arrange
            var textAsDocxFile = Path.Combine(_tempDirectory, "Document.docx");
            File.WriteAllText(textAsDocxFile, "This is plain text, not a DOCX file");

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(textAsDocxFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithCorruptedZip_ShouldThrowException()
        {
            // Arrange
            var corruptedZipFile = Path.Combine(_tempDirectory, "Corrupted.docx");
            File.WriteAllBytes(corruptedZipFile, new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x00, 0x00, 0x00, 0x00 }); // Incomplete ZIP

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(corruptedZipFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithEmptyZip_ShouldThrowException()
        {
            // Arrange
            var emptyZipFile = Path.Combine(_tempDirectory, "Empty.docx");
            File.WriteAllBytes(emptyZipFile, new byte[] { 0x50, 0x4B, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }); // Empty ZIP

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(emptyZipFile);
            });
        }

        #endregion

        #region Invalid Stream Format Tests

        [Test]
        public void DocumentFactory_OpenDocument_WithInvalidStreamFormat_ShouldThrowException()
        {
            // Arrange
            var invalidStream = new MemoryStream();
            var writer = new StreamWriter(invalidStream);
            writer.Write("This is not a valid DOCX stream");
            writer.Flush();
            invalidStream.Position = 0;

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(invalidStream);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithPdfStreamAsDocx_ShouldThrowException()
        {
            // Arrange
            var pdfStream = new MemoryStream();
            var writer = new StreamWriter(pdfStream);
            writer.Write("%PDF-1.4\n1 0 obj\n<<\n/Type /Catalog\n/Pages 2 0 R\n>>\nendobj");
            writer.Flush();
            pdfStream.Position = 0;

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(pdfStream);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithEmptyStream_ShouldThrowException()
        {
            // Arrange
            var emptyStream = new MemoryStream();

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(emptyStream);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithCorruptedStream_ShouldThrowException()
        {
            // Arrange
            var corruptedStream = new MemoryStream();
            var writer = new StreamWriter(corruptedStream);
            writer.Write("PK\x03\x04"); // Partial ZIP header
            writer.Flush();
            corruptedStream.Position = 0;

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(corruptedStream);
            });
        }

        #endregion

        #region PDF Format Error Tests

        [Test]
        public void PdfFactory_OpenDocument_WithInvalidPdfFormat_ShouldReturnFailure()
        {
            // Arrange
            var invalidPdfFile = Path.Combine(_tempDirectory, "Invalid.pdf");
            File.WriteAllText(invalidPdfFile, "This is not a valid PDF file");

            // Act
            var result = PdfFactory.OpenDocument(invalidPdfFile);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithDocxAsPdf_ShouldReturnFailure()
        {
            // Arrange
            var docxAsPdfFile = Path.Combine(_tempDirectory, "Document.pdf");
            File.WriteAllBytes(docxAsPdfFile, new byte[] { 0x50, 0x4B, 0x03, 0x04 }); // ZIP header

            // Act
            var result = PdfFactory.OpenDocument(docxAsPdfFile);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithTextAsPdf_ShouldReturnFailure()
        {
            // Arrange
            var textAsPdfFile = Path.Combine(_tempDirectory, "Document.pdf");
            File.WriteAllText(textAsPdfFile, "This is plain text, not a PDF file");

            // Act
            var result = PdfFactory.OpenDocument(textAsPdfFile);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithCorruptedPdf_ShouldReturnFailure()
        {
            // Arrange
            var corruptedPdfFile = Path.Combine(_tempDirectory, "Corrupted.pdf");
            File.WriteAllBytes(corruptedPdfFile, new byte[] { 0x25, 0x50, 0x44, 0x46, 0x00, 0x00, 0x00, 0x00 }); // Incomplete PDF

            // Act
            var result = PdfFactory.OpenDocument(corruptedPdfFile);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithEmptyPdf_ShouldReturnFailure()
        {
            // Arrange
            var emptyPdfFile = Path.Combine(_tempDirectory, "Empty.pdf");
            File.WriteAllBytes(emptyPdfFile, new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D, 0x31, 0x2E, 0x34, 0x0A }); // PDF header only

            // Act
            var result = PdfFactory.OpenDocument(emptyPdfFile);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        #endregion

        #region File Extension Mismatch Tests

        [Test]
        public void DocumentFactory_OpenDocument_WithWrongExtension_ShouldThrowException()
        {
            // Arrange
            var docxWithPdfExtension = Path.Combine(_tempDirectory, "Document.pdf");
            
            // Create a valid DOCX file but with .pdf extension
            var document = DocumentFactory.CreateDocument();
            document.SaveAs(docxWithPdfExtension);
            document.Dispose();

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(docxWithPdfExtension);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithNoExtension_ShouldThrowException()
        {
            // Arrange
            var docxWithoutExtension = Path.Combine(_tempDirectory, "Document");
            
            // Create a valid DOCX file but without extension
            var document = DocumentFactory.CreateDocument();
            document.SaveAs(docxWithoutExtension);
            document.Dispose();

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(docxWithoutExtension);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithMultipleExtensions_ShouldThrowException()
        {
            // Arrange
            var docxWithMultipleExtensions = Path.Combine(_tempDirectory, "Document.docx.pdf");
            
            // Create a valid DOCX file but with multiple extensions
            var document = DocumentFactory.CreateDocument();
            document.SaveAs(docxWithMultipleExtensions);
            document.Dispose();

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(docxWithMultipleExtensions);
            });
        }

        #endregion

        #region Binary Format Corruption Tests

        [Test]
        public void DocumentFactory_OpenDocument_WithTruncatedFile_ShouldThrowException()
        {
            // Arrange
            var truncatedFile = Path.Combine(_tempDirectory, "Truncated.docx");
            
            // Create a valid DOCX file
            var document = DocumentFactory.CreateDocument();
            document.SaveAs(truncatedFile);
            document.Dispose();

            // Truncate the file
            var fileInfo = new FileInfo(truncatedFile);
            var truncatedSize = fileInfo.Length / 2;
            using (var fileStream = new FileStream(truncatedFile, FileMode.Open, FileAccess.Write))
            {
                fileStream.SetLength(truncatedSize);
            }

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(truncatedFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithModifiedHeader_ShouldThrowException()
        {
            // Arrange
            var modifiedFile = Path.Combine(_tempDirectory, "Modified.docx");
            
            // Create a valid DOCX file
            var document = DocumentFactory.CreateDocument();
            document.SaveAs(modifiedFile);
            document.Dispose();

            // Modify the file header
            using (var fileStream = new FileStream(modifiedFile, FileMode.Open, FileAccess.Write))
            {
                fileStream.Position = 0;
                fileStream.WriteByte(0xFF); // Corrupt the first byte
            }

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(modifiedFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithZeroByteFile_ShouldThrowException()
        {
            // Arrange
            var zeroByteFile = Path.Combine(_tempDirectory, "ZeroByte.docx");
            File.Create(zeroByteFile).Close();

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(zeroByteFile);
            });
        }

        #endregion

        #region Encoding Format Tests

        [Test]
        public void DocumentFactory_OpenDocument_WithInvalidEncoding_ShouldThrowException()
        {
            // Arrange
            var invalidEncodingFile = Path.Combine(_tempDirectory, "InvalidEncoding.docx");
            
            // Create a file with invalid encoding
            using (var fileStream = new FileStream(invalidEncodingFile, FileMode.Create))
            {
                // Write some bytes that don't form a valid ZIP/DOCX structure
                var invalidBytes = new byte[] { 0xFF, 0xFE, 0xFD, 0xFC, 0xFB, 0xFA };
                fileStream.Write(invalidBytes, 0, invalidBytes.Length);
            }

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(invalidEncodingFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithUnicodeBom_ShouldThrowException()
        {
            // Arrange
            var unicodeBomFile = Path.Combine(_tempDirectory, "UnicodeBom.docx");
            
            // Create a file with Unicode BOM
            using (var fileStream = new FileStream(unicodeBomFile, FileMode.Create))
            {
                var unicodeBom = new byte[] { 0xEF, 0xBB, 0xBF };
                fileStream.Write(unicodeBom, 0, unicodeBom.Length);
                fileStream.WriteByte(0x50); // P
                fileStream.WriteByte(0x4B); // K
            }

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(unicodeBomFile);
            });
        }

        #endregion

        #region Stream Format Error Tests

        [Test]
        public void DocumentFactory_OpenDocument_WithInvalidStreamPosition_ShouldThrowException()
        {
            // Arrange
            var validDocxFile = Path.Combine(_tempDirectory, "Valid.docx");
            var document = DocumentFactory.CreateDocument();
            document.SaveAs(validDocxFile);
            document.Dispose();

            var stream = new FileStream(validDocxFile, FileMode.Open, FileAccess.Read);
            stream.Position = stream.Length; // Set position to end

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(stream);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithReadOnlyStream_ShouldThrowException()
        {
            // Arrange
            var validDocxFile = Path.Combine(_tempDirectory, "Valid.docx");
            var document = DocumentFactory.CreateDocument();
            document.SaveAs(validDocxFile);
            document.Dispose();

            var stream = new FileStream(validDocxFile, FileMode.Open, FileAccess.Read);

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(stream);
            });
        }

        #endregion
    }
}
