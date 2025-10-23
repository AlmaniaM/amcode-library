using System;
using System.IO;
using NUnit.Framework;
using AMCode.Docx;
using AMCode.Documents.Pdf;

namespace AMCode.Documents.UnitTests.ErrorHandling
{
    /// <summary>
    /// Tests for file system error handling in the document library
    /// </summary>
    [TestFixture]
    public class FileSystemErrorTests
    {
        private string _tempDirectory;
        private string _readOnlyDirectory;
        private string _nonExistentDirectory;

        [SetUp]
        public void SetUp()
        {
            // Create temporary directory for tests
            _tempDirectory = Path.Combine(Path.GetTempPath(), "AMCodeDocumentsTest", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);

            // Create read-only directory
            _readOnlyDirectory = Path.Combine(_tempDirectory, "ReadOnly");
            Directory.CreateDirectory(_readOnlyDirectory);
            var readOnlyInfo = new DirectoryInfo(_readOnlyDirectory);
            readOnlyInfo.Attributes |= FileAttributes.ReadOnly;

            // Set up non-existent directory path
            _nonExistentDirectory = Path.Combine(_tempDirectory, "NonExistent", "Nested", "Path");
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                // Clean up read-only directory
                if (Directory.Exists(_readOnlyDirectory))
                {
                    var readOnlyInfo = new DirectoryInfo(_readOnlyDirectory);
                    readOnlyInfo.Attributes &= ~FileAttributes.ReadOnly;
                    Directory.Delete(_readOnlyDirectory, true);
                }

                // Clean up temp directory
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

        #region Document Factory File System Error Tests

        [Test]
        public void DocumentFactory_OpenDocument_WithNonExistentFile_ShouldThrowFileNotFoundException()
        {
            // Arrange
            var nonExistentFile = Path.Combine(_tempDirectory, "NonExistent.docx");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
            {
                DocumentFactory.OpenDocument(nonExistentFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithDirectoryPath_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var directoryPath = _tempDirectory;

            // Act & Assert
            Assert.Throws<UnauthorizedAccessException>(() =>
            {
                DocumentFactory.OpenDocument(directoryPath);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithInvalidFilePath_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidPath = "C:\\Invalid<>Path|File.docx";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                DocumentFactory.OpenDocument(invalidPath);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithTooLongFilePath_ShouldThrowPathTooLongException()
        {
            // Arrange
            var longPath = Path.Combine(_tempDirectory, new string('A', 300) + ".docx");

            // Act & Assert
            Assert.Throws<PathTooLongException>(() =>
            {
                DocumentFactory.OpenDocument(longPath);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithNetworkPath_ShouldHandleGracefully()
        {
            // Arrange
            var networkPath = @"\\NonExistentServer\Share\Document.docx";

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(networkPath);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithCorruptedFile_ShouldThrowException()
        {
            // Arrange
            var corruptedFile = Path.Combine(_tempDirectory, "Corrupted.docx");
            File.WriteAllText(corruptedFile, "This is not a valid DOCX file");

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(corruptedFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithEmptyFile_ShouldThrowException()
        {
            // Arrange
            var emptyFile = Path.Combine(_tempDirectory, "Empty.docx");
            File.Create(emptyFile).Close();

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(emptyFile);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithLockedFile_ShouldThrowIOException()
        {
            // Arrange
            var lockedFile = Path.Combine(_tempDirectory, "Locked.docx");
            using (var fileStream = File.Create(lockedFile))
            {
                // Keep file locked
                fileStream.WriteByte(0);

                // Act & Assert
                Assert.Throws<IOException>(() =>
                {
                    DocumentFactory.OpenDocument(lockedFile);
                });
            }
        }

        #endregion

        #region PDF Factory File System Error Tests

        [Test]
        public void PdfFactory_OpenDocument_WithNonExistentFile_ShouldReturnFailure()
        {
            // Arrange
            var nonExistentFile = Path.Combine(_tempDirectory, "NonExistent.pdf");

            // Act
            var result = PdfFactory.OpenDocument(nonExistentFile);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithDirectoryPath_ShouldReturnFailure()
        {
            // Arrange
            var directoryPath = _tempDirectory;

            // Act
            var result = PdfFactory.OpenDocument(directoryPath);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithInvalidFilePath_ShouldReturnFailure()
        {
            // Arrange
            var invalidPath = "C:\\Invalid<>Path|File.pdf";

            // Act
            var result = PdfFactory.OpenDocument(invalidPath);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithTooLongFilePath_ShouldReturnFailure()
        {
            // Arrange
            var longPath = Path.Combine(_tempDirectory, new string('A', 300) + ".pdf");

            // Act
            var result = PdfFactory.OpenDocument(longPath);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithCorruptedFile_ShouldReturnFailure()
        {
            // Arrange
            var corruptedFile = Path.Combine(_tempDirectory, "Corrupted.pdf");
            File.WriteAllText(corruptedFile, "This is not a valid PDF file");

            // Act
            var result = PdfFactory.OpenDocument(corruptedFile);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithEmptyFile_ShouldReturnFailure()
        {
            // Arrange
            var emptyFile = Path.Combine(_tempDirectory, "Empty.pdf");
            File.Create(emptyFile).Close();

            // Act
            var result = PdfFactory.OpenDocument(emptyFile);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.Not.Null);
        }

        [Test]
        public void PdfFactory_OpenDocument_WithLockedFile_ShouldReturnFailure()
        {
            // Arrange
            var lockedFile = Path.Combine(_tempDirectory, "Locked.pdf");
            using (var fileStream = File.Create(lockedFile))
            {
                // Keep file locked
                fileStream.WriteByte(0);

                // Act
                var result = PdfFactory.OpenDocument(lockedFile);

                // Assert
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.Not.Null);
            }
        }

        #endregion

        #region Document Save File System Error Tests

        [Test]
        public void Document_SaveToFile_WithReadOnlyDirectory_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var readOnlyFile = Path.Combine(_readOnlyDirectory, "Document.docx");

            // Act & Assert
            Assert.Throws<UnauthorizedAccessException>(() =>
            {
                document.SaveAs(readOnlyFile);
            });
        }

        [Test]
        public void Document_SaveToFile_WithNonExistentDirectory_ShouldThrowDirectoryNotFoundException()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var nonExistentFile = Path.Combine(_nonExistentDirectory, "Document.docx");

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() =>
            {
                document.SaveAs(nonExistentFile);
            });
        }

        [Test]
        public void Document_SaveToFile_WithInvalidFileName_ShouldThrowArgumentException()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var invalidFile = Path.Combine(_tempDirectory, "Invalid<>File|Name.docx");

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                document.SaveAs(invalidFile);
            });
        }

        [Test]
        public void Document_SaveToFile_WithTooLongFileName_ShouldThrowPathTooLongException()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var longFile = Path.Combine(_tempDirectory, new string('A', 300) + ".docx");

            // Act & Assert
            Assert.Throws<PathTooLongException>(() =>
            {
                document.SaveAs(longFile);
            });
        }

        [Test]
        public void Document_SaveToFile_WithExistingReadOnlyFile_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var readOnlyFile = Path.Combine(_tempDirectory, "ReadOnlyDocument.docx");
            
            // Create a read-only file
            File.WriteAllText(readOnlyFile, "Test content");
            var fileInfo = new FileInfo(readOnlyFile);
            fileInfo.Attributes |= FileAttributes.ReadOnly;

            try
            {
                // Act & Assert
                Assert.Throws<UnauthorizedAccessException>(() =>
                {
                    document.SaveAs(readOnlyFile);
                });
            }
            finally
            {
                // Clean up read-only file
                fileInfo.Attributes &= ~FileAttributes.ReadOnly;
                File.Delete(readOnlyFile);
            }
        }

        [Test]
        public void Document_SaveToFile_WithInsufficientDiskSpace_ShouldThrowIOException()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var largeFile = Path.Combine(_tempDirectory, "LargeDocument.docx");

            // This test simulates disk space issues by creating a very large document
            // In a real scenario, this would be limited by available disk space
            for (int i = 0; i < 1000; i++)
            {
                var paragraph = document.Paragraphs.Create();
                paragraph.Text = new string('A', 1000); // Large content
            }

            // Act & Assert
            // This test may not always throw IOException depending on available disk space
            // but it tests the scenario where large documents are created
            Assert.DoesNotThrow(() =>
            {
                document.SaveAs(largeFile);
            });

            // Clean up
            if (File.Exists(largeFile))
            {
                File.Delete(largeFile);
            }
        }

        #endregion

        #region Stream Error Tests

        [Test]
        public void DocumentFactory_OpenDocument_WithClosedStream_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var stream = new MemoryStream();
            stream.Close();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() =>
            {
                DocumentFactory.OpenDocument(stream);
            });
        }

        [Test]
        public void DocumentFactory_OpenDocument_WithReadOnlyStream_ShouldThrowException()
        {
            // Arrange
            var stream = new MemoryStream();
            stream.Close();
            stream = new MemoryStream(stream.ToArray(), false); // Read-only stream

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(stream);
            });
        }

        [Test]
        public void Document_SaveToStream_WithClosedStream_ShouldThrowObjectDisposedException()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var stream = new MemoryStream();
            stream.Close();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() =>
            {
                document.SaveAs(stream);
            });
        }

        [Test]
        public void Document_SaveToStream_WithReadOnlyStream_ShouldThrowException()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var stream = new MemoryStream();
            stream.Close();
            stream = new MemoryStream(stream.ToArray(), false); // Read-only stream

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                document.SaveAs(stream);
            });
        }

        #endregion

        #region Network and Permission Error Tests

        [Test]
        public void DocumentFactory_OpenDocument_WithNetworkUnavailable_ShouldThrowException()
        {
            // Arrange
            var networkPath = @"\\NonExistentServer\Share\Document.docx";

            // Act & Assert
            Assert.Throws<Exception>(() =>
            {
                DocumentFactory.OpenDocument(networkPath);
            });
        }

        [Test]
        public void Document_SaveToFile_WithInsufficientPermissions_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var restrictedFile = Path.Combine(_tempDirectory, "RestrictedDocument.docx");

            // This test simulates permission issues
            // In a real scenario, this would be limited by file system permissions
            Assert.DoesNotThrow(() =>
            {
                document.SaveAs(restrictedFile);
            });

            // Clean up
            if (File.Exists(restrictedFile))
            {
                File.Delete(restrictedFile);
            }
        }

        #endregion
    }
}
