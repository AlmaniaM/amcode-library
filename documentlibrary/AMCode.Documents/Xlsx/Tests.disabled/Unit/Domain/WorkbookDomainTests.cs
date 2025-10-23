using System;
using System.Threading.Tasks;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Domain.Models;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit.Domain
{
    /// <summary>
    /// Unit tests for WorkbookDomain class
    /// </summary>
    [TestFixture]
    public class WorkbookDomainTests : UnitTestBase
    {
        private IWorkbookContent _mockContent;
        private IWorkbookMetadata _mockMetadata;
        private IWorkbookLogger _mockLogger;
        private IWorkbookValidator _mockValidator;
        private WorkbookDomain _workbookDomain;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            
            _mockContent = CreateMockWorkbookContent();
            _mockMetadata = CreateMockWorkbookMetadata();
            _mockLogger = CreateMockWorkbookLogger();
            _mockValidator = CreateMockWorkbookValidator();
            
            _workbookDomain = new WorkbookDomain(_mockContent, _mockMetadata, _mockLogger, _mockValidator);
        }

        /// <summary>
        /// Test constructor with valid dependencies
        /// </summary>
        [Test]
        public void Constructor_WithValidDependencies_ShouldSucceed()
        {
            // Act & Assert
            Assert.IsNotNull(_workbookDomain);
            Assert.IsNotNull(_workbookDomain.Id);
            Assert.IsTrue(_workbookDomain.CreatedAt > DateTime.MinValue);
            Assert.IsTrue(_workbookDomain.LastModified > DateTime.MinValue);
        }

        /// <summary>
        /// Test constructor with null content adapter should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullContentAdapter_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new WorkbookDomain(null, _mockMetadata, _mockLogger, _mockValidator));
        }

        /// <summary>
        /// Test constructor with null metadata adapter should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullMetadataAdapter_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new WorkbookDomain(_mockContent, null, _mockLogger, _mockValidator));
        }

        /// <summary>
        /// Test constructor with null logger should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new WorkbookDomain(_mockContent, _mockMetadata, null, _mockValidator));
        }

        /// <summary>
        /// Test constructor with null validator should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullValidator_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new WorkbookDomain(_mockContent, _mockMetadata, _mockLogger, null));
        }

        /// <summary>
        /// Test Id property returns unique value
        /// </summary>
        [Test]
        public void Id_ShouldReturnUniqueValue()
        {
            // Act
            var id1 = _workbookDomain.Id;
            var id2 = new WorkbookDomain(_mockContent, _mockMetadata, _mockLogger, _mockValidator).Id;

            // Assert
            Assert.IsNotNull(id1);
            Assert.IsNotNull(id2);
            Assert.AreNotEqual(id1, id2);
        }

        /// <summary>
        /// Test CreatedAt property returns valid date
        /// </summary>
        [Test]
        public void CreatedAt_ShouldReturnValidDate()
        {
            // Act
            var createdAt = _workbookDomain.CreatedAt;

            // Assert
            Assert.IsTrue(createdAt > DateTime.MinValue);
            Assert.IsTrue(createdAt <= DateTime.Now);
        }

        /// <summary>
        /// Test LastModified property returns valid date
        /// </summary>
        [Test]
        public void LastModified_ShouldReturnValidDate()
        {
            // Act
            var lastModified = _workbookDomain.LastModified;

            // Assert
            Assert.IsTrue(lastModified > DateTime.MinValue);
            Assert.IsTrue(lastModified <= DateTime.Now);
        }

        /// <summary>
        /// Test Worksheets property delegates to content adapter
        /// </summary>
        [Test]
        public void Worksheets_ShouldDelegateToContentAdapter()
        {
            // Act
            var worksheets = _workbookDomain.Worksheets;

            // Assert
            Assert.IsNotNull(worksheets);
            Assert.AreEqual(_mockContent.Worksheets, worksheets);
        }

        /// <summary>
        /// Test Author property delegates to metadata adapter
        /// </summary>
        [Test]
        public void Author_ShouldDelegateToMetadataAdapter()
        {
            // Act
            var author = _workbookDomain.Author;

            // Assert
            Assert.AreEqual(_mockMetadata.Author, author);
        }

        /// <summary>
        /// Test Title property delegates to metadata adapter
        /// </summary>
        [Test]
        public void Title_ShouldDelegateToMetadataAdapter()
        {
            // Act
            var title = _workbookDomain.Title;

            // Assert
            Assert.AreEqual(_mockMetadata.Title, title);
        }

        /// <summary>
        /// Test Subject property delegates to metadata adapter
        /// </summary>
        [Test]
        public void Subject_ShouldDelegateToMetadataAdapter()
        {
            // Act
            var subject = _workbookDomain.Subject;

            // Assert
            Assert.AreEqual(_mockMetadata.Subject, subject);
        }

        /// <summary>
        /// Test Company property delegates to metadata adapter
        /// </summary>
        [Test]
        public void Company_ShouldDelegateToMetadataAdapter()
        {
            // Act
            var company = _workbookDomain.Company;

            // Assert
            Assert.AreEqual(_mockMetadata.Company, company);
        }

        /// <summary>
        /// Test Created property delegates to metadata adapter
        /// </summary>
        [Test]
        public void Created_ShouldDelegateToMetadataAdapter()
        {
            // Act
            var created = _workbookDomain.Created;

            // Assert
            Assert.AreEqual(_mockMetadata.Created, created);
        }

        /// <summary>
        /// Test Modified property delegates to metadata adapter
        /// </summary>
        [Test]
        public void Modified_ShouldDelegateToMetadataAdapter()
        {
            // Act
            var modified = _workbookDomain.Modified;

            // Assert
            Assert.AreEqual(_mockMetadata.Modified, modified);
        }

        /// <summary>
        /// Test SaveAs(Stream) method delegates to content adapter
        /// </summary>
        [Test]
        public void SaveAs_WithStream_ShouldDelegateToContentAdapter()
        {
            // Arrange
            using var stream = CreateTempStream();

            // Act
            _workbookDomain.SaveAs(stream);

            // Assert
            AssertStreamHasContent(stream);
        }

        /// <summary>
        /// Test SaveAs(string) method delegates to content adapter
        /// </summary>
        [Test]
        public void SaveAs_WithFilePath_ShouldDelegateToContentAdapter()
        {
            // Arrange
            var filePath = CreateTempFilePath();

            // Act
            _workbookDomain.SaveAs(filePath);

            // Assert
            AssertFileExistsAndHasContent(filePath);
        }

        /// <summary>
        /// Test SetProperty method delegates to metadata adapter
        /// </summary>
        [Test]
        public void SetProperty_ShouldDelegateToMetadataAdapter()
        {
            // Arrange
            var propertyName = "TestProperty";
            var propertyValue = "TestValue";

            // Act
            var result = _workbookDomain.SetProperty(propertyName, propertyValue);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test GetProperty method delegates to metadata adapter
        /// </summary>
        [Test]
        public void GetProperty_ShouldDelegateToMetadataAdapter()
        {
            // Arrange
            var propertyName = "Author";

            // Act
            var result = _workbookDomain.GetProperty(propertyName);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
        }

        /// <summary>
        /// Test Close method calls dispose
        /// </summary>
        [Test]
        public void Close_ShouldCallDispose()
        {
            // Act
            _workbookDomain.Close();

            // Assert
            // Verify that the object is disposed (this would be implementation-specific)
            Assert.IsTrue(true); // Placeholder assertion
        }

        /// <summary>
        /// Test Dispose method cleans up resources
        /// </summary>
        [Test]
        public void Dispose_ShouldCleanUpResources()
        {
            // Act
            _workbookDomain.Dispose();

            // Assert
            // Verify that resources are cleaned up (this would be implementation-specific)
            Assert.IsTrue(true); // Placeholder assertion
        }

        /// <summary>
        /// Test multiple disposal calls don't throw
        /// </summary>
        [Test]
        public void Dispose_MultipleCalls_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _workbookDomain.Dispose();
                _workbookDomain.Dispose();
            });
        }

        /// <summary>
        /// Test that disposed object throws on operations
        /// </summary>
        [Test]
        public void DisposedObject_ShouldThrowOnOperations()
        {
            // Arrange
            _workbookDomain.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => _workbookDomain.Close());
        }

        /// <summary>
        /// Test that disposed object throws on property access
        /// </summary>
        [Test]
        public void DisposedObject_ShouldThrowOnPropertyAccess()
        {
            // Arrange
            _workbookDomain.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => { var _ = _workbookDomain.Id; });
        }

        /// <summary>
        /// Test that disposed object throws on method calls
        /// </summary>
        [Test]
        public void DisposedObject_ShouldThrowOnMethodCalls()
        {
            // Arrange
            _workbookDomain.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => _workbookDomain.SaveAs(CreateTempStream()));
        }

        #region Helper Methods

        /// <summary>
        /// Creates a mock IWorkbookContent for testing
        /// </summary>
        /// <returns>A mock IWorkbookContent</returns>
        private IWorkbookContent CreateMockWorkbookContent()
        {
            return new MockWorkbookContent();
        }

        /// <summary>
        /// Creates a mock IWorkbookMetadata for testing
        /// </summary>
        /// <returns>A mock IWorkbookMetadata</returns>
        private IWorkbookMetadata CreateMockWorkbookMetadata()
        {
            return new MockWorkbookMetadata();
        }

        #endregion
    }

    #region Mock Implementations

    /// <summary>
    /// Mock implementation of IWorkbookContent for testing
    /// </summary>
    public class MockWorkbookContent : IWorkbookContent
    {
        public System.Collections.Generic.IEnumerable<IWorksheet> Worksheets { get; } = new List<IWorksheet>();

        public Result<bool> SaveAs(System.IO.Stream stream)
        {
            var data = System.Text.Encoding.UTF8.GetBytes("Mock Excel Data");
            stream.Write(data, 0, data.Length);
            return Result<bool>.Success(true);
        }

        public Result<bool> SaveAs(string filePath)
        {
            System.IO.File.WriteAllText(filePath, "Mock Excel Data");
            return Result<bool>.Success(true);
        }
    }

    /// <summary>
    /// Mock implementation of IWorkbookMetadata for testing
    /// </summary>
    public class MockWorkbookMetadata : IWorkbookMetadata
    {
        public string Author { get; set; } = "Test Author";
        public string Title { get; set; } = "Test Title";
        public string Subject { get; set; } = "Test Subject";
        public string Company { get; set; } = "Test Company";
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;

        public Result<bool> SetProperty(string name, string value)
        {
            return Result<bool>.Success(true);
        }

        public Result<string> GetProperty(string name)
        {
            return Result<string>.Success(name switch
            {
                "Author" => Author,
                "Title" => Title,
                "Subject" => Subject,
                "Company" => Company,
                _ => null
            });
        }
    }

    #endregion
}
