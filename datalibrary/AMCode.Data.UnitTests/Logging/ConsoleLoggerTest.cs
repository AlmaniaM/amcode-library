using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AMCode.Data.Logging;
using AMCode.Data.Logging.Configuration;
using AMCode.Data.Logging.Infrastructure;
using AMCode.Data.Logging.Infrastructure.Console;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Logging
{
    [TestFixture]
    public class ConsoleLoggerTest
    {
        private ConsoleLogger _logger;
        private ConsoleLoggerProvider _provider;
        private LoggingConfiguration _configuration;
        private ConsoleLoggerConfiguration _consoleConfig;
        private StringWriter _stringWriter;
        private TextWriter _originalOut;

        [SetUp]
        public void SetUp()
        {
            _configuration = new LoggingConfiguration
            {
                MinimumLevel = LogLevel.Trace,
                EnableConsole = true,
                EnableStructuredLogging = false
            };
            _consoleConfig = new ConsoleLoggerConfiguration();
            _provider = new ConsoleLoggerProvider(_configuration, _consoleConfig);
            _logger = new ConsoleLogger("TestCategory", _provider, _configuration, _consoleConfig);

            // Capture console output
            _originalOut = Console.Out;
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [TearDown]
        public void TearDown()
        {
            // Restore console output
            Console.SetOut(_originalOut);
            _stringWriter?.Dispose();
        }

        [Test]
        public void LogInformation_WithMessage_OutputsToConsole()
        {
            // Arrange
            var message = "Test information message";

            // Act
            _logger.LogInformation(message);

            // Assert
            var output = _stringWriter.ToString();
            Assert.That(output, Contains.Substring(message));
            Assert.That(output, Contains.Substring("TestCategory"));
        }

        [Test]
        public void LogError_WithException_OutputsToConsole()
        {
            // Arrange
            var message = "Test error message";
            var exception = new Exception("Test exception");

            // Act
            _logger.LogError(message, exception);

            // Assert
            var output = _stringWriter.ToString();
            Assert.That(output, Contains.Substring(message));
            Assert.That(output, Contains.Substring("TestCategory"));
        }

        [Test]
        public void LogPerformance_WithOperationAndDuration_OutputsToConsole()
        {
            // Arrange
            var operation = "TestOperation";
            var duration = TimeSpan.FromMilliseconds(150);

            // Act
            _logger.LogPerformance(operation, duration);

            // Assert
            var output = _stringWriter.ToString();
            Assert.That(output, Contains.Substring(operation));
            Assert.That(output, Contains.Substring("150"));
        }

        [Test]
        public void Log_WithStructuredLogging_OutputsJson()
        {
            // Arrange
            _configuration.EnableStructuredLogging = true;
            var message = "Test structured message";

            // Act
            _logger.LogInformation(message);

            // Assert
            var output = _stringWriter.ToString();
            Assert.That(output, Contains.Substring("{"));
            Assert.That(output, Contains.Substring("message"));
        }

        [Test]
        public void WithContext_ReturnsNewLoggerWithContext()
        {
            // Arrange
            var key = "TestKey";
            var value = "TestValue";

            // Act
            var scopedLogger = _logger.WithContext(key, value);

            // Assert
            Assert.IsNotNull(scopedLogger);
            Assert.IsNotInstanceOf<NoOpLogger>(scopedLogger);
        }

        [Test]
        public void Log_WithDisabledConsole_DoesNotOutput()
        {
            // Arrange
            _configuration.EnableConsole = false;
            var message = "Test message";

            // Act
            _logger.LogInformation(message);

            // Assert
            var output = _stringWriter.ToString();
            Assert.That(output, Is.Empty);
        }

        [Test]
        public void Log_WithLevelBelowMinimum_DoesNotOutput()
        {
            // Arrange
            _configuration.MinimumLevel = LogLevel.Warning;
            var message = "Debug message";

            // Act
            _logger.LogDebug(message);

            // Assert
            var output = _stringWriter.ToString();
            Assert.That(output, Is.Empty);
        }
    }
}
