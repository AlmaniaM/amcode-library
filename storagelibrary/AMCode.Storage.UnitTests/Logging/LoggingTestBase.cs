using AMCode.Storage.Interfaces;
using AMCode.Storage.UnitTests.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace AMCode.Storage.UnitTests.Logging
{
    /// <summary>
    /// Base class for logging-related unit tests.
    /// Provides common setup and utilities for testing storage logging functionality.
    /// </summary>
    public abstract class LoggingTestBase
    {
        protected MockStorageLogger MockLogger { get; private set; } = null!;
        protected MockStorageOperationLogger MockOperationLogger { get; private set; } = null!;
        protected MockStorageMetricsLogger MockMetricsLogger { get; private set; } = null!;
        protected IServiceProvider ServiceProvider { get; private set; } = null!;

        /// <summary>
        /// Sets up the test environment with mock loggers and dependency injection.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            MockLogger = new MockStorageLogger();
            MockOperationLogger = new MockStorageOperationLogger();
            MockMetricsLogger = new MockStorageMetricsLogger();

            var services = new ServiceCollection();
            
            // Register mock loggers
            services.AddSingleton<IStorageLogger>(MockLogger);
            services.AddSingleton<IStorageOperationLogger>(MockOperationLogger);
            services.AddSingleton<IStorageMetricsLogger>(MockMetricsLogger);

            // Register Microsoft.Extensions.Logging with console provider
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Cleans up the test environment.
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            MockLogger?.Clear();
            MockOperationLogger?.Clear();
            MockMetricsLogger?.Clear();
            ServiceProvider?.Dispose();
        }

        /// <summary>
        /// Asserts that a specific number of information logs were recorded.
        /// </summary>
        /// <param name="expectedCount">The expected number of information logs.</param>
        protected void AssertInformationLogCount(int expectedCount)
        {
            Assert.AreEqual(expectedCount, MockLogger.InformationLogs.Count, 
                $"Expected {expectedCount} information logs, but found {MockLogger.InformationLogs.Count}");
        }

        /// <summary>
        /// Asserts that a specific number of warning logs were recorded.
        /// </summary>
        /// <param name="expectedCount">The expected number of warning logs.</param>
        protected void AssertWarningLogCount(int expectedCount)
        {
            Assert.AreEqual(expectedCount, MockLogger.WarningLogs.Count, 
                $"Expected {expectedCount} warning logs, but found {MockLogger.WarningLogs.Count}");
        }

        /// <summary>
        /// Asserts that a specific number of error logs were recorded.
        /// </summary>
        /// <param name="expectedCount">The expected number of error logs.</param>
        protected void AssertErrorLogCount(int expectedCount)
        {
            Assert.AreEqual(expectedCount, MockLogger.ErrorLogs.Count, 
                $"Expected {expectedCount} error logs, but found {MockLogger.ErrorLogs.Count}");
        }

        /// <summary>
        /// Asserts that a specific number of file operation logs were recorded.
        /// </summary>
        /// <param name="expectedCount">The expected number of file operation logs.</param>
        protected void AssertFileOperationLogCount(int expectedCount)
        {
            Assert.AreEqual(expectedCount, MockOperationLogger.FileOperationLogs.Count, 
                $"Expected {expectedCount} file operation logs, but found {MockOperationLogger.FileOperationLogs.Count}");
        }

        /// <summary>
        /// Asserts that a specific number of performance logs were recorded.
        /// </summary>
        /// <param name="expectedCount">The expected number of performance logs.</param>
        protected void AssertPerformanceLogCount(int expectedCount)
        {
            Assert.AreEqual(expectedCount, MockOperationLogger.PerformanceLogs.Count, 
                $"Expected {expectedCount} performance logs, but found {MockOperationLogger.PerformanceLogs.Count}");
        }

        /// <summary>
        /// Asserts that a specific number of operation metrics were recorded.
        /// </summary>
        /// <param name="expectedCount">The expected number of operation metrics.</param>
        protected void AssertOperationMetricsCount(int expectedCount)
        {
            Assert.AreEqual(expectedCount, MockMetricsLogger.OperationMetrics.Count, 
                $"Expected {expectedCount} operation metrics, but found {MockMetricsLogger.OperationMetrics.Count}");
        }

        /// <summary>
        /// Asserts that an information log contains specific text.
        /// </summary>
        /// <param name="expectedText">The text that should be contained in the log.</param>
        protected void AssertInformationLogContains(string expectedText)
        {
            Assert.IsTrue(MockLogger.InformationLogs.Any(log => log.Contains(expectedText)), 
                $"Expected to find information log containing '{expectedText}', but found: {string.Join(", ", MockLogger.InformationLogs)}");
        }

        /// <summary>
        /// Asserts that an error log contains specific text.
        /// </summary>
        /// <param name="expectedText">The text that should be contained in the log.</param>
        protected void AssertErrorLogContains(string expectedText)
        {
            Assert.IsTrue(MockLogger.ErrorLogs.Any(log => log.Contains(expectedText)), 
                $"Expected to find error log containing '{expectedText}', but found: {string.Join(", ", MockLogger.ErrorLogs)}");
        }

        /// <summary>
        /// Asserts that a file operation log exists for a specific operation and file.
        /// </summary>
        /// <param name="operation">The expected operation.</param>
        /// <param name="fileName">The expected file name.</param>
        protected void AssertFileOperationLogExists(string operation, string fileName)
        {
            Assert.IsTrue(MockOperationLogger.FileOperationLogs.Any(log => 
                log.Operation == operation && log.FileName == fileName), 
                $"Expected to find file operation log for operation '{operation}' and file '{fileName}'");
        }

        /// <summary>
        /// Asserts that a performance log exists for a specific operation.
        /// </summary>
        /// <param name="operation">The expected operation.</param>
        protected void AssertPerformanceLogExists(string operation)
        {
            Assert.IsTrue(MockOperationLogger.PerformanceLogs.Any(log => log.Operation == operation), 
                $"Expected to find performance log for operation '{operation}'");
        }

        /// <summary>
        /// Asserts that an operation metric exists for a specific operation and storage type.
        /// </summary>
        /// <param name="operation">The expected operation.</param>
        /// <param name="storageType">The expected storage type.</param>
        protected void AssertOperationMetricExists(string operation, string storageType)
        {
            Assert.IsTrue(MockMetricsLogger.OperationMetrics.Any(metric => 
                metric.Operation == operation && metric.StorageType == storageType), 
                $"Expected to find operation metric for operation '{operation}' and storage type '{storageType}'");
        }
    }
}
