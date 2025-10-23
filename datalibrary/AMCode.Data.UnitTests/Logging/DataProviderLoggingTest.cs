using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Data;
using AMCode.Data.Logging;
using AMCode.Data.Logging.Configuration;
using AMCode.Data.Logging.Infrastructure;
using AMCode.Data.Logging.Infrastructure.Console;
using Moq;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Logging
{
    [TestFixture]
    public class DataProviderLoggingTest
    {
        private Mock<IDbExecuteFactory> _mockDbExecuteFactory;
        private Mock<IExpandoObjectDataProviderFactory> _mockExpandoProviderFactory;
        private Mock<IGenericDataProviderFactory> _mockGenericProviderFactory;
        private Mock<IDbExecute> _mockDbExecute;
        private Mock<IExpandoObjectDataProvider> _mockExpandoProvider;
        private Mock<IGenericDataProvider> _mockGenericProvider;
        private DataProvider _dataProvider;
        private ConsoleLogger _logger;
        private StringWriter _stringWriter;
        private TextWriter _originalOut;

        [SetUp]
        public void SetUp()
        {
            // Setup mocks
            _mockDbExecuteFactory = new Mock<IDbExecuteFactory>();
            _mockExpandoProviderFactory = new Mock<IExpandoObjectDataProviderFactory>();
            _mockGenericProviderFactory = new Mock<IGenericDataProviderFactory>();
            _mockDbExecute = new Mock<IDbExecute>();
            _mockExpandoProvider = new Mock<IExpandoObjectDataProvider>();
            _mockGenericProvider = new Mock<IGenericDataProvider>();

            // Setup factory returns
            _mockDbExecuteFactory.Setup(x => x.Create()).Returns(_mockDbExecute.Object);
            _mockExpandoProviderFactory.Setup(x => x.Create()).Returns(_mockExpandoProvider.Object);
            _mockGenericProviderFactory.Setup(x => x.Create()).Returns(_mockGenericProvider.Object);

            // Setup logger
            var configuration = new LoggingConfiguration
            {
                MinimumLevel = LogLevel.Trace,
                EnableConsole = true,
                EnableStructuredLogging = false
            };
            var consoleConfig = new ConsoleLoggerConfiguration();
            var provider = new ConsoleLoggerProvider(configuration, consoleConfig);
            _logger = new ConsoleLogger("DataProvider", provider, configuration, consoleConfig);

            // Capture console output
            _originalOut = Console.Out;
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);

            // Create DataProvider with logger
            _dataProvider = new DataProvider(
                _mockDbExecuteFactory.Object,
                _mockExpandoProviderFactory.Object,
                _mockGenericProviderFactory.Object,
                _logger
            );
        }

        [TearDown]
        public void TearDown()
        {
            Console.SetOut(_originalOut);
            _stringWriter?.Dispose();
        }

        [Test]
        public async Task ExecuteAsync_WithLogging_LogsInformationAndPerformance()
        {
            // Arrange
            var query = "SELECT * FROM TestTable";
            _mockDbExecute.Setup(x => x.ExecuteAsync(query, It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

            // Act
            await _dataProvider.ExecuteAsync(query);

            // Assert
            var output = _stringWriter.ToString();
            Assert.That(output, Contains.Substring("Executing query"));
            Assert.That(output, Contains.Substring("QueryExecution"));
            Assert.That(output, Contains.Substring(query));
        }

        [Test]
        public async Task ExecuteAsync_WithException_LogsError()
        {
            // Arrange
            var query = "SELECT * FROM TestTable";
            var exception = new Exception("Database error");
            _mockDbExecute.Setup(x => x.ExecuteAsync(query, It.IsAny<CancellationToken>()))
                         .ThrowsAsync(exception);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _dataProvider.ExecuteAsync(query));

            var output = _stringWriter.ToString();
            Assert.That(output, Contains.Substring("Query execution failed"));
            Assert.That(output, Contains.Substring(query));
        }

        [Test]
        public async Task GetExpandoListAsync_WithLogging_LogsInformationAndPerformance()
        {
            // Arrange
            var query = "SELECT * FROM TestTable";
            var expectedResult = new List<ExpandoObject> { new ExpandoObject() };
            _mockExpandoProvider.Setup(x => x.GetExpandoListAsync(query, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(expectedResult);

            // Act
            var result = await _dataProvider.GetExpandoListAsync(query);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
            var output = _stringWriter.ToString();
            Assert.That(output, Contains.Substring("Executing ExpandoObject query"));
            Assert.That(output, Contains.Substring("ExpandoObjectQuery"));
            Assert.That(output, Contains.Substring(query));
        }

        [Test]
        public async Task GetListOfAsync_WithLogging_LogsInformationAndPerformance()
        {
            // Arrange
            var query = "SELECT * FROM TestTable";
            var expectedResult = new List<TestModel> { new TestModel() };
            _mockGenericProvider.Setup(x => x.GetListOfAsync<TestModel>(query, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(expectedResult);

            // Act
            var result = await _dataProvider.GetListOfAsync<TestModel>(query);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
            var output = _stringWriter.ToString();
            Assert.That(output, Contains.Substring("Executing generic query"));
            Assert.That(output, Contains.Substring("GenericQuery"));
            Assert.That(output, Contains.Substring(query));
            Assert.That(output, Contains.Substring("TestModel"));
        }

        [Test]
        public async Task GetValueOfAsync_WithLogging_LogsInformationAndPerformance()
        {
            // Arrange
            var query = "SELECT COUNT(*) FROM TestTable";
            var columnName = "Count";
            var expectedValue = 42;
            _mockGenericProvider.Setup(x => x.GetValueOfAsync<int>(columnName, query, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(expectedValue);

            // Act
            var result = await _dataProvider.GetValueOfAsync<int>(columnName, query);

            // Assert
            Assert.That(result, Is.EqualTo(expectedValue));
            var output = _stringWriter.ToString();
            Assert.That(output, Contains.Substring("Executing value query"));
            Assert.That(output, Contains.Substring("ValueQuery"));
            Assert.That(output, Contains.Substring(query));
            Assert.That(output, Contains.Substring(columnName));
        }

        [Test]
        public void DataProvider_WithNoLogger_UsesNoOpLogger()
        {
            // Arrange & Act
            var dataProvider = new DataProvider(
                _mockDbExecuteFactory.Object,
                _mockExpandoProviderFactory.Object,
                _mockGenericProviderFactory.Object
            );

            // Assert
            Assert.DoesNotThrowAsync(() => dataProvider.ExecuteAsync("SELECT 1"));
        }

        public class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
