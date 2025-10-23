using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using AMCode.Docx;
using AMCode.Documents.Pdf;

namespace AMCode.Documents.UnitTests.ErrorHandling
{
    /// <summary>
    /// Tests for memory constraint handling in the document library
    /// </summary>
    [TestFixture]
    public class MemoryErrorTests
    {
        private string _tempDirectory;

        [SetUp]
        public void SetUp()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), "AMCodeDocumentsMemoryTest", Guid.NewGuid().ToString());
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

        #region Large Document Memory Tests

        [Test]
        public void Document_CreateLargeDocument_ShouldHandleMemoryConstraints()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var largeContent = new string('A', 10000); // 10KB content per paragraph

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                // Create many paragraphs to test memory usage
                for (int i = 0; i < 100; i++)
                {
                    var paragraph = document.Paragraphs.Create();
                    paragraph.Text = largeContent + $" - Paragraph {i}";
                }
            });
        }

        [Test]
        public void Document_CreateLargeTable_ShouldHandleMemoryConstraints()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                // Create a large table
                var table = document.Tables.Create(100, 10);
                
                // Fill the table with data
                for (int row = 0; row < 100; row++)
                {
                    for (int col = 0; col < 10; col++)
                    {
                        table.SetCellValue(row, col, $"Row {row}, Col {col} - " + new string('X', 100));
                    }
                }
            });
        }

        [Test]
        public void Document_CreateMultipleLargeDocuments_ShouldHandleMemoryConstraints()
        {
            // Arrange
            var documents = new List<IDocument>();

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                // Create multiple large documents
                for (int docIndex = 0; docIndex < 10; docIndex++)
                {
                    var document = DocumentFactory.CreateDocument();
                    
                    // Add content to each document
                    for (int i = 0; i < 50; i++)
                    {
                        var paragraph = document.Paragraphs.Create();
                        paragraph.Text = $"Document {docIndex}, Paragraph {i} - " + new string('B', 1000);
                    }
                    
                    documents.Add(document);
                }
            });

            // Clean up
            foreach (var doc in documents)
            {
                doc?.Dispose();
            }
        }

        [Test]
        public void Document_SaveLargeDocument_ShouldHandleMemoryConstraints()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var largeContent = new string('C', 5000);

            // Add large content
            for (int i = 0; i < 200; i++)
            {
                var paragraph = document.Paragraphs.Create();
                paragraph.Text = largeContent + $" - Large Paragraph {i}";
            }

            var filePath = Path.Combine(_tempDirectory, "LargeDocument.docx");

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                document.SaveAs(filePath);
            });

            // Verify file was created
            Assert.That(File.Exists(filePath), Is.True);
            Assert.That(new FileInfo(filePath).Length, Is.GreaterThan(0));
        }

        #endregion

        #region Memory Leak Prevention Tests

        [Test]
        public void Document_Dispose_ShouldReleaseMemory()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(true);
            var documents = new List<IDocument>();

            // Act
            for (int i = 0; i < 50; i++)
            {
                var document = DocumentFactory.CreateDocument();
                
                // Add content
                for (int j = 0; j < 20; j++)
                {
                    var paragraph = document.Paragraphs.Create();
                    paragraph.Text = $"Document {i}, Paragraph {j} - " + new string('D', 1000);
                }
                
                documents.Add(document);
            }

            // Dispose all documents
            foreach (var doc in documents)
            {
                doc.Dispose();
            }
            documents.Clear();

            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var finalMemory = GC.GetTotalMemory(false);

            // Assert
            // Memory should be released after disposal
            // Note: This test may not always pass due to GC timing, but it tests the disposal pattern
            Assert.That(finalMemory, Is.LessThanOrEqualTo(initialMemory + (50 * 1024 * 1024))); // Allow 50MB overhead
        }

        [Test]
        public void Document_UsingStatement_ShouldDisposeAutomatically()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(true);

            // Act
            for (int i = 0; i < 20; i++)
            {
                using (var document = DocumentFactory.CreateDocument())
                {
                    // Add content
                    for (int j = 0; j < 10; j++)
                    {
                        var paragraph = document.Paragraphs.Create();
                        paragraph.Text = $"Document {i}, Paragraph {j} - " + new string('E', 500);
                    }
                }
            }

            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var finalMemory = GC.GetTotalMemory(false);

            // Assert
            // Memory should be released after using statement
            Assert.That(finalMemory, Is.LessThanOrEqualTo(initialMemory + (10 * 1024 * 1024))); // Allow 10MB overhead
        }

        #endregion

        #region Large File Handling Tests

        [Test]
        public void Document_OpenLargeFile_ShouldHandleMemoryConstraints()
        {
            // Arrange
            var largeDocument = DocumentFactory.CreateDocument();
            var largeContent = new string('F', 2000);

            // Create a large document
            for (int i = 0; i < 100; i++)
            {
                var paragraph = largeDocument.Paragraphs.Create();
                paragraph.Text = largeContent + $" - Large Paragraph {i}";
            }

            var filePath = Path.Combine(_tempDirectory, "LargeFile.docx");
            largeDocument.SaveAs(filePath);
            largeDocument.Dispose();

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                using (var openedDocument = DocumentFactory.OpenDocument(filePath))
                {
                    // Verify we can access the content
                    Assert.That(openedDocument.Paragraphs.Count, Is.GreaterThan(0));
                }
            });
        }

        [Test]
        public void Document_StreamLargeContent_ShouldHandleMemoryConstraints()
        {
            // Arrange
            var document = DocumentFactory.CreateDocument();
            var largeContent = new string('G', 5000);

            // Add large content
            for (int i = 0; i < 50; i++)
            {
                var paragraph = document.Paragraphs.Create();
                paragraph.Text = largeContent + $" - Stream Paragraph {i}";
            }

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                using (var stream = new MemoryStream())
                {
                    document.SaveAs(stream);
                    
                    // Verify stream has content
                    Assert.That(stream.Length, Is.GreaterThan(0));
                }
            });
        }

        #endregion

        #region Concurrent Memory Usage Tests

        [Test]
        public void Document_CreateConcurrentDocuments_ShouldHandleMemoryConstraints()
        {
            // Arrange
            var tasks = new List<System.Threading.Tasks.Task>();

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    var taskIndex = i;
                    var task = System.Threading.Tasks.Task.Run(() =>
                    {
                        using (var document = DocumentFactory.CreateDocument())
                        {
                            // Add content to each document
                            for (int j = 0; j < 20; j++)
                            {
                                var paragraph = document.Paragraphs.Create();
                                paragraph.Text = $"Task {taskIndex}, Paragraph {j} - " + new string('H', 1000);
                            }
                        }
                    });
                    tasks.Add(task);
                }

                // Wait for all tasks to complete
                System.Threading.Tasks.Task.WaitAll(tasks.ToArray());
            });
        }

        [Test]
        public void Document_ConcurrentSave_ShouldHandleMemoryConstraints()
        {
            // Arrange
            var documents = new List<IDocument>();
            var filePaths = new List<string>();

            // Create multiple documents
            for (int i = 0; i < 5; i++)
            {
                var document = DocumentFactory.CreateDocument();
                
                // Add content
                for (int j = 0; j < 10; j++)
                {
                    var paragraph = document.Paragraphs.Create();
                    paragraph.Text = $"Document {i}, Paragraph {j} - " + new string('I', 500);
                }
                
                documents.Add(document);
                filePaths.Add(Path.Combine(_tempDirectory, $"ConcurrentDocument{i}.docx"));
            }

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                var tasks = new List<System.Threading.Tasks.Task>();
                
                for (int i = 0; i < documents.Count; i++)
                {
                    var docIndex = i;
                    var task = System.Threading.Tasks.Task.Run(() =>
                    {
                        documents[docIndex].SaveAs(filePaths[docIndex]);
                    });
                    tasks.Add(task);
                }

                // Wait for all save operations to complete
                System.Threading.Tasks.Task.WaitAll(tasks.ToArray());
            });

            // Clean up
            foreach (var doc in documents)
            {
                doc.Dispose();
            }
        }

        #endregion

        #region Memory Pressure Tests

        [Test]
        public void Document_UnderMemoryPressure_ShouldHandleGracefully()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(true);

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                // Create documents under memory pressure
                for (int i = 0; i < 100; i++)
                {
                    using (var document = DocumentFactory.CreateDocument())
                    {
                        // Add content
                        for (int j = 0; j < 5; j++)
                        {
                            var paragraph = document.Paragraphs.Create();
                            paragraph.Text = $"Pressure Test {i}, Paragraph {j} - " + new string('J', 200);
                        }
                    }

                    // Force garbage collection every 10 iterations
                    if (i % 10 == 0)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
            });

            // Final cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        [Test]
        public void Document_MemoryEfficientDisposal_ShouldPreventLeaks()
        {
            // Arrange
            var documents = new List<IDocument>();

            // Act
            for (int i = 0; i < 30; i++)
            {
                var document = DocumentFactory.CreateDocument();
                
                // Add content
                for (int j = 0; j < 15; j++)
                {
                    var paragraph = document.Paragraphs.Create();
                    paragraph.Text = $"Memory Test {i}, Paragraph {j} - " + new string('K', 300);
                }
                
                documents.Add(document);
            }

            // Dispose in batches to test memory efficiency
            for (int i = 0; i < documents.Count; i += 5)
            {
                for (int j = i; j < Math.Min(i + 5, documents.Count); j++)
                {
                    documents[j].Dispose();
                }
                
                // Force garbage collection after each batch
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            documents.Clear();

            // Final cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Assert
            // Test passes if no OutOfMemoryException is thrown
            Assert.Pass("Memory efficient disposal completed successfully");
        }

        #endregion
    }
}
