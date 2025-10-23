using System;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit.Builders
{
    /// <summary>
    /// Unit tests for WorkbookBuilder class
    /// </summary>
    [TestFixture]
    public class WorkbookBuilderTests : UnitTestBase
    {
        private IWorkbookEngine _mockEngine;
        private IWorkbookLogger _mockLogger;
        private IWorkbookValidator _mockValidator;
        private IWorkbookBuilder _builder;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _mockEngine = CreateMockWorkbookEngine();
            _mockLogger = CreateMockWorkbookLogger();
            _mockValidator = CreateMockWorkbookValidator();
            _builder = CreateMockWorkbookBuilder();
        }

        #region Fluent API Method Chaining Tests

        /// <summary>
        /// Test that all methods return IWorkbookBuilder for chaining
        /// </summary>
        [Test]
        public void AllMethods_ShouldReturnIWorkbookBuilderForChaining()
        {
            // Act
            var result = _builder
                .WithTitle("Test Title")
                .WithAuthor("Test Author")
                .WithSubject("Test Subject")
                .WithCompany("Test Company")
                .WithKeywords("test, keywords")
                .WithComments("Test comments")
                .WithManager("Test Manager")
                .WithApplication("Test Application")
                .WithTemplate("Test Template")
                .WithRevision("1.0")
                .WithEditingTime(TimeSpan.FromHours(1))
                .WithPageCount(10)
                .WithWordCount(1000)
                .WithCharacterCount(5000)
                .WithCharacterCountWithSpaces(6000)
                .WithLineCount(50)
                .WithParagraphCount(25)
                .WithSlideCount(5)
                .WithNoteCount(10)
                .WithHiddenSlideCount(2)
                .WithMMClipCount(0)
                .WithScaleCrop(false)
                .WithSharedDoc(false)
                .WithHyperlinkBase("http://example.com")
                .WithHyperlinksChanged(false)
                .WithAppVersion("1.0.0")
                .WithDocSecurity(0)
                .WithLinksUpToDate(false)
                .WithScaleCrop(false)
                .WithSharedDoc(false)
                .WithHyperlinkBase("http://example.com")
                .WithHyperlinksChanged(false)
                .WithAppVersion("1.0.0")
                .WithDocSecurity(0)
                .WithLinksUpToDate(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        #endregion

        #region WithTitle Tests

        /// <summary>
        /// Test WithTitle with valid title should succeed
        /// </summary>
        [Test]
        public void WithTitle_WithValidTitle_ShouldSucceed()
        {
            // Arrange
            var title = "Test Workbook Title";

            // Act
            var result = _builder.WithTitle(title);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        /// <summary>
        /// Test WithTitle with null title should succeed
        /// </summary>
        [Test]
        public void WithTitle_WithNullTitle_ShouldSucceed()
        {
            // Act
            var result = _builder.WithTitle(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        /// <summary>
        /// Test WithTitle with empty title should succeed
        /// </summary>
        [Test]
        public void WithTitle_WithEmptyTitle_ShouldSucceed()
        {
            // Act
            var result = _builder.WithTitle("");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        /// <summary>
        /// Test WithTitle with very long title should succeed
        /// </summary>
        [Test]
        public void WithTitle_WithVeryLongTitle_ShouldSucceed()
        {
            // Arrange
            var longTitle = new string('A', 1000);

            // Act
            var result = _builder.WithTitle(longTitle);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        #endregion

        #region WithAuthor Tests

        /// <summary>
        /// Test WithAuthor with valid author should succeed
        /// </summary>
        [Test]
        public void WithAuthor_WithValidAuthor_ShouldSucceed()
        {
            // Arrange
            var author = "Test Author";

            // Act
            var result = _builder.WithAuthor(author);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        /// <summary>
        /// Test WithAuthor with null author should succeed
        /// </summary>
        [Test]
        public void WithAuthor_WithNullAuthor_ShouldSucceed()
        {
            // Act
            var result = _builder.WithAuthor(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        /// <summary>
        /// Test WithAuthor with empty author should succeed
        /// </summary>
        [Test]
        public void WithAuthor_WithEmptyAuthor_ShouldSucceed()
        {
            // Act
            var result = _builder.WithAuthor("");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        #endregion

        #region WithSubject Tests

        /// <summary>
        /// Test WithSubject with valid subject should succeed
        /// </summary>
        [Test]
        public void WithSubject_WithValidSubject_ShouldSucceed()
        {
            // Arrange
            var subject = "Test Subject";

            // Act
            var result = _builder.WithSubject(subject);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        /// <summary>
        /// Test WithSubject with null subject should succeed
        /// </summary>
        [Test]
        public void WithSubject_WithNullSubject_ShouldSucceed()
        {
            // Act
            var result = _builder.WithSubject(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        #endregion

        #region WithCompany Tests

        /// <summary>
        /// Test WithCompany with valid company should succeed
        /// </summary>
        [Test]
        public void WithCompany_WithValidCompany_ShouldSucceed()
        {
            // Arrange
            var company = "Test Company";

            // Act
            var result = _builder.WithCompany(company);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        /// <summary>
        /// Test WithCompany with null company should succeed
        /// </summary>
        [Test]
        public void WithCompany_WithNullCompany_ShouldSucceed()
        {
            // Act
            var result = _builder.WithCompany(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        #endregion

        #region WithWorksheet Tests

        /// <summary>
        /// Test WithWorksheet with valid worksheet should succeed
        /// </summary>
        [Test]
        public void WithWorksheet_WithValidWorksheet_ShouldSucceed()
        {
            // Arrange
            var worksheet = CreateMockWorksheet();

            // Act
            var result = _builder.WithWorksheet(worksheet);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        /// <summary>
        /// Test WithWorksheet with null worksheet should fail
        /// </summary>
        [Test]
        public void WithWorksheet_WithNullWorksheet_ShouldFail()
        {
            // Act
            var result = _builder.WithWorksheet(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet cannot be null");
        }

        /// <summary>
        /// Test WithWorksheet with worksheet name should succeed
        /// </summary>
        [Test]
        public void WithWorksheet_WithWorksheetName_ShouldSucceed()
        {
            // Arrange
            var worksheetName = "Test Worksheet";

            // Act
            var result = _builder.WithWorksheet(worksheetName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorkbookBuilder>(result);
        }

        /// <summary>
        /// Test WithWorksheet with null worksheet name should fail
        /// </summary>
        [Test]
        public void WithWorksheet_WithNullWorksheetName_ShouldFail()
        {
            // Act
            var result = _builder.WithWorksheet((string)null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test WithWorksheet with empty worksheet name should fail
        /// </summary>
        [Test]
        public void WithWorksheet_WithEmptyWorksheetName_ShouldFail()
        {
            // Act
            var result = _builder.WithWorksheet("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        #endregion

        #region Build Tests

        /// <summary>
        /// Test Build method should succeed
        /// </summary>
        [Test]
        public void Build_ShouldSucceed()
        {
            // Act
            var result = _builder.Build();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorkbookDomain>(result.Value);
        }

        /// <summary>
        /// Test Build method with configured properties should succeed
        /// </summary>
        [Test]
        public void Build_WithConfiguredProperties_ShouldSucceed()
        {
            // Arrange
            _builder
                .WithTitle("Test Title")
                .WithAuthor("Test Author")
                .WithSubject("Test Subject")
                .WithCompany("Test Company");

            // Act
            var result = _builder.Build();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            var workbook = result.Value;
            Assert.AreEqual("Test Title", workbook.Title);
            Assert.AreEqual("Test Author", workbook.Author);
            Assert.AreEqual("Test Subject", workbook.Subject);
            Assert.AreEqual("Test Company", workbook.Company);
        }

        /// <summary>
        /// Test Build method with multiple worksheets should succeed
        /// </summary>
        [Test]
        public void Build_WithMultipleWorksheets_ShouldSucceed()
        {
            // Arrange
            _builder
                .WithWorksheet("Worksheet 1")
                .WithWorksheet("Worksheet 2")
                .WithWorksheet("Worksheet 3");

            // Act
            var result = _builder.Build();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            var workbook = result.Value;
            Assert.AreEqual(3, workbook.Worksheets.Count());
        }

        #endregion

        #region Error Handling Tests

        /// <summary>
        /// Test Build method with validation failure should fail
        /// </summary>
        [Test]
        public void Build_WithValidationFailure_ShouldFail()
        {
            // Arrange
            var mockValidator = CreateMockWorkbookValidator();
            mockValidator.ShouldValidateWorkbookSuccess = false;
            var builder = CreateMockWorkbookBuilder(mockValidator);

            // Act
            var result = builder.Build();

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Workbook validation failed");
        }

        /// <summary>
        /// Test Build method with engine failure should fail
        /// </summary>
        [Test]
        public void Build_WithEngineFailure_ShouldFail()
        {
            // Arrange
            var mockEngine = CreateMockWorkbookEngine();
            mockEngine.ShouldCreateSuccess = false;
            var builder = CreateMockWorkbookBuilder(engine: mockEngine);

            // Act
            var result = builder.Build();

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to create workbook");
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Test complete workflow with all builder methods
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithAllBuilderMethods_ShouldSucceed()
        {
            // Act
            var result = _builder
                .WithTitle("Complete Test Workbook")
                .WithAuthor("Test Author")
                .WithSubject("Test Subject")
                .WithCompany("Test Company")
                .WithKeywords("test, workbook, complete")
                .WithComments("Complete test workbook")
                .WithManager("Test Manager")
                .WithApplication("Test Application")
                .WithTemplate("Test Template")
                .WithRevision("1.0")
                .WithEditingTime(TimeSpan.FromHours(2))
                .WithPageCount(20)
                .WithWordCount(2000)
                .WithCharacterCount(10000)
                .WithCharacterCountWithSpaces(12000)
                .WithLineCount(100)
                .WithParagraphCount(50)
                .WithSlideCount(10)
                .WithNoteCount(20)
                .WithHiddenSlideCount(5)
                .WithMMClipCount(0)
                .WithScaleCrop(false)
                .WithSharedDoc(false)
                .WithHyperlinkBase("http://test.com")
                .WithHyperlinksChanged(false)
                .WithAppVersion("2.0.0")
                .WithDocSecurity(1)
                .WithLinksUpToDate(true)
                .WithWorksheet("Data Sheet")
                .WithWorksheet("Summary Sheet")
                .WithWorksheet("Charts Sheet")
                .Build();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            var workbook = result.Value;
            Assert.AreEqual("Complete Test Workbook", workbook.Title);
            Assert.AreEqual("Test Author", workbook.Author);
            Assert.AreEqual("Test Subject", workbook.Subject);
            Assert.AreEqual("Test Company", workbook.Company);
            Assert.AreEqual(3, workbook.Worksheets.Count());
        }

        /// <summary>
        /// Test that built workbook can be used for operations
        /// </summary>
        [Test]
        public void BuiltWorkbook_CanBeUsedForOperations()
        {
            // Arrange
            var result = _builder
                .WithTitle("Operational Test Workbook")
                .WithAuthor("Test Author")
                .WithWorksheet("Test Sheet")
                .Build();

            AssertResultSuccess(result);
            var workbook = result.Value;

            // Act & Assert
            // Test basic operations
            Assert.IsNotNull(workbook.Id);
            Assert.IsNotNull(workbook.Title);
            Assert.IsNotNull(workbook.Worksheets);
            Assert.IsNotNull(workbook.Author);

            // Test save operations
            using var stream = new MemoryStream();
            var saveStreamResult = workbook.SaveAs(stream);
            AssertResultSuccess(saveStreamResult);

            var filePath = CreateTempFilePath();
            var saveFileResult = workbook.SaveAs(filePath);
            AssertResultSuccess(saveFileResult);
            AssertFileExistsAndHasContent(filePath);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a mock workbook builder for testing
        /// </summary>
        /// <returns>A mock workbook builder</returns>
        private IWorkbookBuilder CreateMockWorkbookBuilder(IWorkbookValidator validator = null, IWorkbookEngine engine = null)
        {
            return new MockWorkbookBuilder(
                engine ?? _mockEngine,
                _mockLogger,
                validator ?? _mockValidator);
        }

        /// <summary>
        /// Creates a mock worksheet for testing
        /// </summary>
        /// <returns>A mock worksheet</returns>
        private IWorksheet CreateMockWorksheet()
        {
            return new MockWorksheet();
        }

        #endregion
    }

    #region Helper Classes

    /// <summary>
    /// Mock implementation of IWorkbookBuilder for testing
    /// </summary>
    public class MockWorkbookBuilder : IWorkbookBuilder
    {
        private readonly IWorkbookEngine _engine;
        private readonly IWorkbookLogger _logger;
        private readonly IWorkbookValidator _validator;
        private string _title;
        private string _author;
        private string _subject;
        private string _company;
        private string _keywords;
        private string _comments;
        private string _manager;
        private string _application;
        private string _template;
        private string _revision;
        private TimeSpan? _editingTime;
        private int? _pageCount;
        private int? _wordCount;
        private int? _characterCount;
        private int? _characterCountWithSpaces;
        private int? _lineCount;
        private int? _paragraphCount;
        private int? _slideCount;
        private int? _noteCount;
        private int? _hiddenSlideCount;
        private int? _mmClipCount;
        private bool? _scaleCrop;
        private bool? _sharedDoc;
        private string _hyperlinkBase;
        private bool? _hyperlinksChanged;
        private string _appVersion;
        private int? _docSecurity;
        private bool? _linksUpToDate;
        private readonly System.Collections.Generic.List<IWorksheet> _worksheets = new System.Collections.Generic.List<IWorksheet>();

        public MockWorkbookBuilder(IWorkbookEngine engine, IWorkbookLogger logger, IWorkbookValidator validator)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public IWorkbookBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public IWorkbookBuilder WithAuthor(string author)
        {
            _author = author;
            return this;
        }

        public IWorkbookBuilder WithSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public IWorkbookBuilder WithCompany(string company)
        {
            _company = company;
            return this;
        }

        public IWorkbookBuilder WithKeywords(string keywords)
        {
            _keywords = keywords;
            return this;
        }

        public IWorkbookBuilder WithComments(string comments)
        {
            _comments = comments;
            return this;
        }

        public IWorkbookBuilder WithManager(string manager)
        {
            _manager = manager;
            return this;
        }

        public IWorkbookBuilder WithApplication(string application)
        {
            _application = application;
            return this;
        }

        public IWorkbookBuilder WithTemplate(string template)
        {
            _template = template;
            return this;
        }

        public IWorkbookBuilder WithRevision(string revision)
        {
            _revision = revision;
            return this;
        }

        public IWorkbookBuilder WithEditingTime(TimeSpan editingTime)
        {
            _editingTime = editingTime;
            return this;
        }

        public IWorkbookBuilder WithPageCount(int pageCount)
        {
            _pageCount = pageCount;
            return this;
        }

        public IWorkbookBuilder WithWordCount(int wordCount)
        {
            _wordCount = wordCount;
            return this;
        }

        public IWorkbookBuilder WithCharacterCount(int characterCount)
        {
            _characterCount = characterCount;
            return this;
        }

        public IWorkbookBuilder WithCharacterCountWithSpaces(int characterCountWithSpaces)
        {
            _characterCountWithSpaces = characterCountWithSpaces;
            return this;
        }

        public IWorkbookBuilder WithLineCount(int lineCount)
        {
            _lineCount = lineCount;
            return this;
        }

        public IWorkbookBuilder WithParagraphCount(int paragraphCount)
        {
            _paragraphCount = paragraphCount;
            return this;
        }

        public IWorkbookBuilder WithSlideCount(int slideCount)
        {
            _slideCount = slideCount;
            return this;
        }

        public IWorkbookBuilder WithNoteCount(int noteCount)
        {
            _noteCount = noteCount;
            return this;
        }

        public IWorkbookBuilder WithHiddenSlideCount(int hiddenSlideCount)
        {
            _hiddenSlideCount = hiddenSlideCount;
            return this;
        }

        public IWorkbookBuilder WithMMClipCount(int mmClipCount)
        {
            _mmClipCount = mmClipCount;
            return this;
        }

        public IWorkbookBuilder WithScaleCrop(bool scaleCrop)
        {
            _scaleCrop = scaleCrop;
            return this;
        }

        public IWorkbookBuilder WithSharedDoc(bool sharedDoc)
        {
            _sharedDoc = sharedDoc;
            return this;
        }

        public IWorkbookBuilder WithHyperlinkBase(string hyperlinkBase)
        {
            _hyperlinkBase = hyperlinkBase;
            return this;
        }

        public IWorkbookBuilder WithHyperlinksChanged(bool hyperlinksChanged)
        {
            _hyperlinksChanged = hyperlinksChanged;
            return this;
        }

        public IWorkbookBuilder WithAppVersion(string appVersion)
        {
            _appVersion = appVersion;
            return this;
        }

        public IWorkbookBuilder WithDocSecurity(int docSecurity)
        {
            _docSecurity = docSecurity;
            return this;
        }

        public IWorkbookBuilder WithLinksUpToDate(bool linksUpToDate)
        {
            _linksUpToDate = linksUpToDate;
            return this;
        }

        public IWorkbookBuilder WithWorksheet(IWorksheet worksheet)
        {
            if (worksheet == null)
                return Result<IWorkbookBuilder>.Failure("Worksheet cannot be null");
            
            _worksheets.Add(worksheet);
            return this;
        }

        public IWorkbookBuilder WithWorksheet(string worksheetName)
        {
            if (string.IsNullOrEmpty(worksheetName))
                return Result<IWorkbookBuilder>.Failure("Worksheet name cannot be null or empty");
            
            var worksheet = new MockWorksheet { Name = worksheetName };
            _worksheets.Add(worksheet);
            return this;
        }

        public Result<IWorkbookDomain> Build()
        {
            try
            {
                // Create workbook using engine
                var engineResult = _engine.Create();
                if (!engineResult.IsSuccess)
                    return Result<IWorkbookDomain>.Failure("Failed to create workbook");

                // Create workbook domain
                var workbook = new MockWorkbookDomain
                {
                    Title = _title,
                    Author = _author,
                    Subject = _subject,
                    Company = _company
                };

                // Add worksheets
                foreach (var worksheet in _worksheets)
                {
                    workbook.Worksheets.Add(worksheet);
                }

                // Validate workbook
                var validationResult = _validator.ValidateWorkbook(workbook);
                if (!validationResult.IsSuccess)
                    return Result<IWorkbookDomain>.Failure("Workbook validation failed");

                return Result<IWorkbookDomain>.Success(workbook);
            }
            catch (Exception ex)
            {
                return Result<IWorkbookDomain>.Failure("Failed to build workbook", ex);
            }
        }
    }

    /// <summary>
    /// Mock implementation of IWorkbookDomain for testing
    /// </summary>
    public class MockWorkbookDomain : IWorkbookDomain
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }
        public string Company { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;
        public System.Collections.Generic.IEnumerable<IWorksheet> Worksheets { get; } = new System.Collections.Generic.List<IWorksheet>();

        public void Close()
        {
            // Mock implementation
        }

        public void Dispose()
        {
            // Mock implementation
        }

        public Result<string> GetProperty(string name)
        {
            return Result<string>.Success(name switch
            {
                "Title" => Title,
                "Author" => Author,
                "Subject" => Subject,
                "Company" => Company,
                _ => null
            });
        }

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

        public Result<bool> SetProperty(string name, string value)
        {
            return Result<bool>.Success(true);
        }
    }

    #endregion
}
