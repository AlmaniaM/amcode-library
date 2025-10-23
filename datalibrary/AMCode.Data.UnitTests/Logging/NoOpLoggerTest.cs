using System;
using System.Collections.Generic;
using AMCode.Data.Logging;
using AMCode.Data.Logging.Infrastructure;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Logging
{
    [TestFixture]
    public class NoOpLoggerTest
    {
        private NoOpLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new NoOpLogger();
        }

        [Test]
        public void Log_WithAllParameters_DoesNotThrow()
        {
            // Arrange
            var message = "Test message";
            var exception = new Exception("Test exception");
            var context = new { TestProperty = "TestValue" };

            // Act & Assert
            Assert.DoesNotThrow(() => _logger.Log(LogLevel.Information, message, exception, context));
        }

        [Test]
        public void Log_WithProperties_DoesNotThrow()
        {
            // Arrange
            var message = "Test message";
            var properties = new Dictionary<string, object> { ["Key"] = "Value" };
            var exception = new Exception("Test exception");

            // Act & Assert
            Assert.DoesNotThrow(() => _logger.Log(LogLevel.Information, message, properties, exception));
        }

        [Test]
        public void LogInformation_DoesNotThrow()
        {
            // Arrange
            var message = "Information message";
            var context = new { TestProperty = "TestValue" };

            // Act & Assert
            Assert.DoesNotThrow(() => _logger.LogInformation(message, context));
        }

        [Test]
        public void LogWarning_DoesNotThrow()
        {
            // Arrange
            var message = "Warning message";
            var context = new { TestProperty = "TestValue" };

            // Act & Assert
            Assert.DoesNotThrow(() => _logger.LogWarning(message, context));
        }

        [Test]
        public void LogError_DoesNotThrow()
        {
            // Arrange
            var message = "Error message";
            var exception = new Exception("Test exception");
            var context = new { TestProperty = "TestValue" };

            // Act & Assert
            Assert.DoesNotThrow(() => _logger.LogError(message, exception, context));
        }

        [Test]
        public void LogDebug_DoesNotThrow()
        {
            // Arrange
            var message = "Debug message";
            var context = new { TestProperty = "TestValue" };

            // Act & Assert
            Assert.DoesNotThrow(() => _logger.LogDebug(message, context));
        }

        [Test]
        public void LogTrace_DoesNotThrow()
        {
            // Arrange
            var message = "Trace message";
            var context = new { TestProperty = "TestValue" };

            // Act & Assert
            Assert.DoesNotThrow(() => _logger.LogTrace(message, context));
        }

        [Test]
        public void LogPerformance_DoesNotThrow()
        {
            // Arrange
            var operation = "TestOperation";
            var duration = TimeSpan.FromMilliseconds(100);
            var context = new { TestProperty = "TestValue" };

            // Act & Assert
            Assert.DoesNotThrow(() => _logger.LogPerformance(operation, duration, context));
        }

        [Test]
        public void WithContext_ReturnsSelf()
        {
            // Arrange
            var key = "TestKey";
            var value = "TestValue";

            // Act
            var result = _logger.WithContext(key, value);

            // Assert
            Assert.AreSame(_logger, result);
        }

        [Test]
        public void WithContext_WithDictionary_ReturnsSelf()
        {
            // Arrange
            var context = new Dictionary<string, object> { ["Key"] = "Value" };

            // Act
            var result = _logger.WithContext(context);

            // Assert
            Assert.AreSame(_logger, result);
        }
    }
}
