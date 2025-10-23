using System;
using System.Collections.Generic;

namespace AMCode.Data.Logging.Infrastructure
{
    /// <summary>
    /// Generic typed logger implementation
    /// </summary>
    public class Logger<T> : ILogger<T>
    {
        private readonly ILogger _logger;

        public Logger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Log(LogLevel level, string message, Exception exception = null, object context = null)
        {
            _logger.Log(level, message, exception, context);
        }

        public void Log(LogLevel level, string message, IDictionary<string, object> properties, Exception exception = null)
        {
            _logger.Log(level, message, properties, exception);
        }

        public void LogInformation(string message, object context = null)
        {
            _logger.LogInformation(message, context);
        }

        public void LogWarning(string message, object context = null)
        {
            _logger.LogWarning(message, context);
        }

        public void LogError(string message, Exception exception = null, object context = null)
        {
            _logger.LogError(message, exception, context);
        }

        public void LogDebug(string message, object context = null)
        {
            _logger.LogDebug(message, context);
        }

        public void LogTrace(string message, object context = null)
        {
            _logger.LogTrace(message, context);
        }

        public void LogPerformance(string operation, TimeSpan duration, object context = null)
        {
            _logger.LogPerformance(operation, duration, context);
        }

        public ILogger WithContext(string key, object value)
        {
            return _logger.WithContext(key, value);
        }

        public ILogger WithContext(IDictionary<string, object> context)
        {
            return _logger.WithContext(context);
        }
    }
}
