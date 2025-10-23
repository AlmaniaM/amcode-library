using AMCode.Storage.Infrastructure.Logging;
using AMCode.Storage.UnitTests.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace AMCode.Storage.UnitTests.Logging
{
    /// <summary>
    /// Unit tests for the StorageLogger class.
    /// </summary>
    [TestFixture]
    public class StorageLoggerTests : LoggingTestBase
    {
        private StorageLogger _storageLogger = null!;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            
            // Create a mock ILogger<StorageLogger> for testing
            var mockLogger = new MockLogger<StorageLogger>();
            _storageLogger = new StorageLogger(mockLogger);
        }

        [Test]
        public void ShouldLogInformation()
        {
            // Act
            _storageLogger.LogInformation("Test message {Value}", "test");

            // Assert
            AssertInformationLogCount(1);
            AssertInformationLogContains("Test message test");
        }

        [Test]
        public void ShouldLogWarning()
        {
            // Act
            _storageLogger.LogWarning("Test warning {Value}", "test");

            // Assert
            AssertWarningLogCount(1);
            Assert.IsTrue(MockLogger.WarningLogs.Any(log => log.Contains("Test warning test")));
        }

        [Test]
        public void ShouldLogError()
        {
            // Act
            _storageLogger.LogError("Test error {Value}", "test");

            // Assert
            AssertErrorLogCount(1);
            AssertErrorLogContains("Test error test");
        }

        [Test]
        public void ShouldLogErrorWithException()
        {
            // Arrange
            var exception = new Exception("Test exception");

            // Act
            _storageLogger.LogError(exception, "Test error {Value}", "test");

            // Assert
            AssertErrorLogCount(1);
            AssertErrorLogContains("Test error test");
            AssertErrorLogContains("Exception: Test exception");
            Assert.AreEqual(1, MockLogger.LoggedExceptions.Count);
            Assert.AreEqual(exception, MockLogger.LoggedExceptions[0]);
        }

        [Test]
        public void ShouldLogDebug()
        {
            // Act
            _storageLogger.LogDebug("Test debug {Value}", "test");

            // Assert
            Assert.AreEqual(1, MockLogger.DebugLogs.Count);
            Assert.IsTrue(MockLogger.DebugLogs.Any(log => log.Contains("Test debug test")));
        }

        [Test]
        public void ShouldLogTrace()
        {
            // Act
            _storageLogger.LogTrace("Test trace {Value}", "test");

            // Assert
            Assert.AreEqual(1, MockLogger.TraceLogs.Count);
            Assert.IsTrue(MockLogger.TraceLogs.Any(log => log.Contains("Test trace test")));
        }

        [Test]
        public void ShouldHandleNullArguments()
        {
            // Act & Assert - Should not throw
            Assert.DoesNotThrow(() => _storageLogger.LogInformation("Test message", null));
            Assert.DoesNotThrow(() => _storageLogger.LogWarning("Test warning", null));
            Assert.DoesNotThrow(() => _storageLogger.LogError("Test error", null));
        }
    }

    /// <summary>
    /// Mock implementation of ILogger<T> for testing purposes.
    /// </summary>
    public class MockLogger<T> : ILogger<T>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // This is a simple mock that doesn't capture logs
            // In a real test, you might want to capture these for verification
        }
    }
}
