using System;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Infrastructure.Adapters;
using AMCode.Documents.Xlsx.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit.Infrastructure
{
    /// <summary>
    /// Unit tests for WorkbookMetadataAdapter class
    /// </summary>
    [TestFixture]
    public class WorkbookMetadataAdapterTests : UnitTestBase
    {
        private IWorkbookProperties _mockProperties;
        private WorkbookMetadataAdapter _adapter;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _mockProperties = CreateMockWorkbookProperties();
            _adapter = new WorkbookMetadataAdapter(_mockProperties);
        }

        #region Constructor Tests

        /// <summary>
        /// Test constructor with valid IWorkbookProperties should succeed
        /// </summary>
        [Test]
        public void Constructor_WithValidProperties_ShouldSucceed()
        {
            // Act & Assert
            Assert.IsNotNull(_adapter);
        }

        /// <summary>
        /// Test constructor with null IWorkbookProperties should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullProperties_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new WorkbookMetadataAdapter(null));
        }

        #endregion

        #region Property Getter Tests

        /// <summary>
        /// Test Author property delegates to underlying properties
        /// </summary>
        [Test]
        public void Author_ShouldDelegateToUnderlyingProperties()
        {
            // Arrange
            var expectedAuthor = "Test Author";
            _mockProperties.Author = expectedAuthor;

            // Act
            var author = _adapter.Author;

            // Assert
            Assert.AreEqual(expectedAuthor, author);
        }

        /// <summary>
        /// Test Title property delegates to underlying properties
        /// </summary>
        [Test]
        public void Title_ShouldDelegateToUnderlyingProperties()
        {
            // Arrange
            var expectedTitle = "Test Title";
            _mockProperties.Title = expectedTitle;

            // Act
            var title = _adapter.Title;

            // Assert
            Assert.AreEqual(expectedTitle, title);
        }

        /// <summary>
        /// Test Subject property delegates to underlying properties
        /// </summary>
        [Test]
        public void Subject_ShouldDelegateToUnderlyingProperties()
        {
            // Arrange
            var expectedSubject = "Test Subject";
            _mockProperties.Subject = expectedSubject;

            // Act
            var subject = _adapter.Subject;

            // Assert
            Assert.AreEqual(expectedSubject, subject);
        }

        /// <summary>
        /// Test Company property delegates to underlying properties
        /// </summary>
        [Test]
        public void Company_ShouldDelegateToUnderlyingProperties()
        {
            // Arrange
            var expectedCompany = "Test Company";
            _mockProperties.Company = expectedCompany;

            // Act
            var company = _adapter.Company;

            // Assert
            Assert.AreEqual(expectedCompany, company);
        }

        /// <summary>
        /// Test Created property delegates to underlying properties
        /// </summary>
        [Test]
        public void Created_ShouldDelegateToUnderlyingProperties()
        {
            // Arrange
            var expectedCreated = DateTime.Now.AddDays(-1);
            _mockProperties.Created = expectedCreated;

            // Act
            var created = _adapter.Created;

            // Assert
            Assert.AreEqual(expectedCreated, created);
        }

        /// <summary>
        /// Test Modified property delegates to underlying properties
        /// </summary>
        [Test]
        public void Modified_ShouldDelegateToUnderlyingProperties()
        {
            // Arrange
            var expectedModified = DateTime.Now;
            _mockProperties.Modified = expectedModified;

            // Act
            var modified = _adapter.Modified;

            // Assert
            Assert.AreEqual(expectedModified, modified);
        }

        #endregion

        #region SetProperty Tests

        /// <summary>
        /// Test SetProperty with valid property name and value should succeed
        /// </summary>
        [Test]
        public void SetProperty_WithValidProperty_ShouldSucceed()
        {
            // Arrange
            var propertyName = "Author";
            var propertyValue = "New Author";

            // Act
            var result = _adapter.SetProperty(propertyName, propertyValue);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetProperty with null property name should fail
        /// </summary>
        [Test]
        public void SetProperty_WithNullPropertyName_ShouldFail()
        {
            // Act
            var result = _adapter.SetProperty(null, "Value");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Property name cannot be null or empty");
        }

        /// <summary>
        /// Test SetProperty with empty property name should fail
        /// </summary>
        [Test]
        public void SetProperty_WithEmptyPropertyName_ShouldFail()
        {
            // Act
            var result = _adapter.SetProperty("", "Value");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Property name cannot be null or empty");
        }

        /// <summary>
        /// Test SetProperty with whitespace property name should fail
        /// </summary>
        [Test]
        public void SetProperty_WithWhitespacePropertyName_ShouldFail()
        {
            // Act
            var result = _adapter.SetProperty("   ", "Value");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Property name cannot be null or empty");
        }

        /// <summary>
        /// Test SetProperty with null property value should succeed
        /// </summary>
        [Test]
        public void SetProperty_WithNullPropertyValue_ShouldSucceed()
        {
            // Arrange
            var propertyName = "Author";

            // Act
            var result = _adapter.SetProperty(propertyName, null);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetProperty with empty property value should succeed
        /// </summary>
        [Test]
        public void SetProperty_WithEmptyPropertyValue_ShouldSucceed()
        {
            // Arrange
            var propertyName = "Author";

            // Act
            var result = _adapter.SetProperty(propertyName, "");

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetProperty with various property names should succeed
        /// </summary>
        [TestCase("Author")]
        [TestCase("Title")]
        [TestCase("Subject")]
        [TestCase("Company")]
        [TestCase("Keywords")]
        [TestCase("Comments")]
        [TestCase("Manager")]
        [TestCase("Application")]
        [TestCase("Template")]
        [TestCase("Revision")]
        public void SetProperty_WithVariousPropertyNames_ShouldSucceed(string propertyName)
        {
            // Act
            var result = _adapter.SetProperty(propertyName, "Test Value");

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region GetProperty Tests

        /// <summary>
        /// Test GetProperty with valid property name should succeed
        /// </summary>
        [Test]
        public void GetProperty_WithValidPropertyName_ShouldSucceed()
        {
            // Arrange
            var propertyName = "Author";
            var expectedValue = "Test Author";
            _mockProperties.Author = expectedValue;

            // Act
            var result = _adapter.GetProperty(propertyName);

            // Assert
            AssertResultSuccess(result);
            Assert.AreEqual(expectedValue, result.Value);
        }

        /// <summary>
        /// Test GetProperty with null property name should fail
        /// </summary>
        [Test]
        public void GetProperty_WithNullPropertyName_ShouldFail()
        {
            // Act
            var result = _adapter.GetProperty(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Property name cannot be null or empty");
        }

        /// <summary>
        /// Test GetProperty with empty property name should fail
        /// </summary>
        [Test]
        public void GetProperty_WithEmptyPropertyName_ShouldFail()
        {
            // Act
            var result = _adapter.GetProperty("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Property name cannot be null or empty");
        }

        /// <summary>
        /// Test GetProperty with whitespace property name should fail
        /// </summary>
        [Test]
        public void GetProperty_WithWhitespacePropertyName_ShouldFail()
        {
            // Act
            var result = _adapter.GetProperty("   ");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Property name cannot be null or empty");
        }

        /// <summary>
        /// Test GetProperty with non-existent property name should return null
        /// </summary>
        [Test]
        public void GetProperty_WithNonExistentPropertyName_ShouldReturnNull()
        {
            // Arrange
            var propertyName = "NonExistentProperty";

            // Act
            var result = _adapter.GetProperty(propertyName);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNull(result.Value);
        }

        /// <summary>
        /// Test GetProperty with various property names should succeed
        /// </summary>
        [TestCase("Author")]
        [TestCase("Title")]
        [TestCase("Subject")]
        [TestCase("Company")]
        [TestCase("Keywords")]
        [TestCase("Comments")]
        [TestCase("Manager")]
        [TestCase("Application")]
        [TestCase("Template")]
        [TestCase("Revision")]
        public void GetProperty_WithVariousPropertyNames_ShouldSucceed(string propertyName)
        {
            // Act
            var result = _adapter.GetProperty(propertyName);

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region Error Handling Tests

        /// <summary>
        /// Test SetProperty with underlying properties throwing exception should handle gracefully
        /// </summary>
        [Test]
        public void SetProperty_WithUnderlyingPropertiesThrowingException_ShouldHandleGracefully()
        {
            // Arrange
            var exceptionThrowingProperties = new ExceptionThrowingWorkbookProperties();
            var adapter = new WorkbookMetadataAdapter(exceptionThrowingProperties);

            // Act
            var result = adapter.SetProperty("Author", "Value");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to set property");
        }

        /// <summary>
        /// Test GetProperty with underlying properties throwing exception should handle gracefully
        /// </summary>
        [Test]
        public void GetProperty_WithUnderlyingPropertiesThrowingException_ShouldHandleGracefully()
        {
            // Arrange
            var exceptionThrowingProperties = new ExceptionThrowingWorkbookProperties();
            var adapter = new WorkbookMetadataAdapter(exceptionThrowingProperties);

            // Act
            var result = adapter.GetProperty("Author");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to get property");
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Test complete workflow with valid operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithValidOperations_ShouldSucceed()
        {
            // Arrange
            var propertyName = "Author";
            var propertyValue = "Test Author";

            // Act & Assert
            // Test property setting
            var setResult = _adapter.SetProperty(propertyName, propertyValue);
            AssertResultSuccess(setResult);

            // Test property getting
            var getResult = _adapter.GetProperty(propertyName);
            AssertResultSuccess(getResult);
            Assert.AreEqual(propertyValue, getResult.Value);

            // Test property access
            var author = _adapter.Author;
            Assert.IsNotNull(author);
        }

        /// <summary>
        /// Test property synchronization between set and get
        /// </summary>
        [Test]
        public void PropertySynchronization_BetweenSetAndGet_ShouldWork()
        {
            // Arrange
            var testData = new[]
            {
                ("Author", "Test Author"),
                ("Title", "Test Title"),
                ("Subject", "Test Subject"),
                ("Company", "Test Company")
            };

            // Act & Assert
            foreach (var (name, value) in testData)
            {
                // Set property
                var setResult = _adapter.SetProperty(name, value);
                AssertResultSuccess(setResult);

                // Get property
                var getResult = _adapter.GetProperty(name);
                AssertResultSuccess(getResult);
                Assert.AreEqual(value, getResult.Value);
            }
        }

        #endregion
    }

    #region Helper Classes

    /// <summary>
    /// Mock workbook properties that throws exceptions for testing error handling
    /// </summary>
    public class ExceptionThrowingWorkbookProperties : IWorkbookProperties
    {
        public string Author { get; set; } = "Test Author";
        public string Title { get; set; } = "Test Title";
        public string Subject { get; set; } = "Test Subject";
        public string Company { get; set; } = "Test Company";
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;

        public string GetProperty(string name)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public void SetProperty(string name, string value)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }
    }

    #endregion
}
